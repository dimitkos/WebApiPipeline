using Handlers;
using System.Web.Http;

namespace MessageHandlers
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new FullPipelineTimerHandler());
            config.MessageHandlers.Add(new ApiKeyHeaderHandler());
            config.MessageHandlers.Add(new HttpMethodOverrideHandler());
            config.MessageHandlers.Add(new ForwardedHeadersHandler());
            config.MessageHandlers.Add(new ClientIpAddressHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
