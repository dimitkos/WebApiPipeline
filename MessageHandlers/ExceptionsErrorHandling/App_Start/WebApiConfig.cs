using ExceptionsErrorHandling.Filters;
using System.Web.Http;

namespace ExceptionsErrorHandling
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Filters.Add(new NotImplementedExceptionFilter());
            // Web API routes
            config.MapHttpAttributeRoutes();

        }
    }
}
