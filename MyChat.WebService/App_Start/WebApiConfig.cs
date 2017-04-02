using System.Web.Http;

namespace MyChat.WebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
