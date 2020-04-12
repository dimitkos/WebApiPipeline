using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Filters.Filters
{
    public enum ClientCacheControl
    {
        Public,//can be cached by intermidiate device
        Private,//can be cached only from browser
        NoCache//nocache
    }

    // TODO: Decide if your filter should allow multiple instances per controller or
    //       per-method; set AllowMultiple to true if so
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ClientCacheControlFilterAttribute : ActionFilterAttribute
    {
        // TODO: If you need constructor arguments, create properties to hold them
        //       and public constructors that accept them.
        public ClientCacheControl CacheType;
        public double CacheSeconds;

        public ClientCacheControlFilterAttribute(double cacheSeconds = 60.0)
        {
            CacheType = ClientCacheControl.Private;
            CacheSeconds = cacheSeconds;
        }

        public ClientCacheControlFilterAttribute(ClientCacheControl cacheType, double cacheSeconds)
        {
            CacheType = cacheType;
            CacheSeconds = cacheSeconds;
            if (cacheType == ClientCacheControl.NoCache)
                cacheSeconds = -1;
        }

        public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            // STEP 2: Call the rest of the action filter chain
            await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            // STEP 3: Any logic you want to do AFTER the other action filters, and AFTER
            //         the action method itself is called.
            if(CacheType == ClientCacheControl.NoCache)
            {
                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoStore = true
                };

                //create date if no present
                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                    actionExecutedContext.Response.Headers.Date = DateTimeOffset.UtcNow;

                actionExecutedContext.Response.Content.Headers.Expires = 
                    actionExecutedContext.Response.Headers.Date;
            }
            else
            {
                actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    Public = (CacheType == ClientCacheControl.Public),
                    Private = (CacheType == ClientCacheControl.Private),
                    NoCache = false,
                    MaxAge = TimeSpan.FromSeconds(CacheSeconds)
                };

                if (!actionExecutedContext.Response.Headers.Date.HasValue)
                    actionExecutedContext.Response.Headers.Date = DateTimeOffset.UtcNow;

                actionExecutedContext.Response.Content.Headers.Expires =
                    actionExecutedContext.Response.Headers.Date.Value.AddSeconds(CacheSeconds);
            }

        }
    }
}