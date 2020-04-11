using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Handlers
{
    public class HttpMethodOverrideHandler : DelegatingHandler
    {
        //readonly string[] _methods = { "DELETE", "PATCH", "PUT" , "HEAD" , "VIEW" };
        //const string _header = "X-HTTP-Method-Override";

        //protected override Task<HttpResponseMessage> SendAsync(
        //HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    string customMethod = null;
        //    if (request.Method == HttpMethod.Post && request.Headers.Contains(_header))
        //    {
        //        customMethod = request.Headers.GetValues(_header).FirstOrDefault();

        //        if(_methods.Contains(customMethod))
        //        {
        //            request.Method = new HttpMethod(customMethod);
        //        }              
        //    }
        //    return base.SendAsync(request, cancellationToken);
        //}

        /// <summary>
        /// Names of http methods that are valid to substitute
        /// </summary>
        readonly string[] _validMethods = {
            HttpMethod.Put.Method,
            HttpMethod.Delete.Method,
            HttpMethod.Head.Method,
            "VIEW", "PATCH"
        };

        /// <summary>
        /// HTTP header name to process
        /// </summary>
        const string _header = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // STEP 1: Global message-level logic that must be executed BEFORE the request
            //          is sent on to the action method

            // X-HTTP-Method-Override header requires POST verb
            if (request.Method.Equals(HttpMethod.Post) && request.Headers.Contains(_header))
            {
                // Check for a valid new method from the list
                var method = request.Headers.GetValues(_header).FirstOrDefault();
                if (_validMethods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method to the override one
                    request.Method = new HttpMethod(method.ToUpperInvariant());
                }
            }
            // Collapse STEPs 2-4 since this handler doesn't do any response processing.
            return base.SendAsync(request, cancellationToken);
        }
    }
}