using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Handlers
{
    /// <summary>
    /// Message handler to retrieve any incoming client IP address, taking into account 
    /// any load balancers or other proxy servers.
    public class ClientIpAddressHandler : DelegatingHandler
    {
        const string _fwdHeader = "Forwarded";
        public const string _fwdForHeader = "X-Forwarded-For";

        /// <summary>
        /// Message handler to retrieve the incoming client ip address,
        /// accounting for any load balancer in use.
        /// </summary>
        protected override Task<HttpResponseMessage> SendAsync(
              HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string ip = null;
            // X-Forwarded-For header - the first one in the comma-separated string is the original client if more than one
            if (request.Headers.Contains(_fwdForHeader))
            {
                ip = request.Headers.GetValues(_fwdForHeader).FirstOrDefault(s => !String.IsNullOrEmpty(s));
                if (!String.IsNullOrEmpty(ip) && ip.Contains(","))
                    ip = ip.Substring(0, ip.IndexOf(','));
            }
            if (String.IsNullOrEmpty(ip) && request.Headers.Contains(_fwdHeader))
            {
                var fwd = request.Headers.GetValues(_fwdHeader)
                    .FirstOrDefault(s => !String.IsNullOrEmpty(s))
                    .Split(';')
                    .Select(s => s.Trim());
                // syntax for the Forwarded header: Forwarded: by=<identifier>; for=<identifier>; host=<host>; proto=<http|https>
                ip = fwd.FirstOrDefault(s => s.ToLowerInvariant().StartsWith("for="));
                if (!String.IsNullOrEmpty(ip))
                    ip = ip.Substring(4);
            }
            // try the http context to see what it has, if none of the standard headers have worked out
            if (String.IsNullOrEmpty(ip))
            {
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    var ctx = request.Properties["MS_HttpContext"] as HttpContextBase;
                    if (ctx != null)
                        ip = ctx.Request.UserHostAddress;
                }
            }

            // store it off if we found it
            if (!String.IsNullOrEmpty(ip))
                request.Properties.Add(_fwdForHeader, ip);

            // continue the handler chain
            return base.SendAsync(request, cancellationToken);
        }       
    }

    /// <summary>
    /// http request extension for retrieving client ip
    /// </summary>
    public static class HttpRequestMessageClientIpAddressExtension
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request == null)
                return null;
            if (request.Properties.TryGetValue(ClientIpAddressHandler._fwdForHeader, out object ip))
            {
                return (string)ip;
            }
            return null;
        }
    }
}