using System;
using System.Web.Http;

namespace ExceptionsErrorHandling
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //see also in web.config
            //if ia have a critical error ,we dont display the stack trace , it is good option for production enviroment.
            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy =
                IncludeErrorDetailPolicy.Default;

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        /// <summary>
        /// For errors outside of the web api pipeline - start up and shut down errors,
        /// plus exceptions that bubble up from the global exception handler/loggers
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            // since this is the handler of last resort - 
            //  you should probably log the error somewhere that will get noticed!
            Context.Trace.Write("ERROR-- WebApiApplication.Application_OnError reached: "
                + ex.ToString());
        }
    }
}
