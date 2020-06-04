using ExceptionsErrorHandling.ExceptionsHandlers;
using ExceptionsErrorHandling.Filters;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ExceptionsErrorHandling
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Filters.Add(new NotImplementedExceptionFilter());
            // Web API routes
            config.MapHttpAttributeRoutes();

        }
    }
}
