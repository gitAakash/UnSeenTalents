using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace UnseentalentsApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.Add(new UploadMultipartMediaTypeFormatter());
            config.Routes.MapHttpRoute(
               name: "ActionApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { controller = "UnseentalentApi", id = RouteParameter.Optional }
           );
        }
    }

    public class UploadMultipartMediaTypeFormatter : MediaTypeFormatter
    {
      public UploadMultipartMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }
    }
}
