using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
using System.Net.Http;

namespace InventoryMgr
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Categories",
                routeTemplate: "api/Categories/{userId}",
                defaults: new { controller = "Categories", action = "GetAll" }
            );

            config.Routes.MapHttpRoute(
                name: "InventoryItems",
                routeTemplate: "api/InventoryItems/{userId}",
                defaults: new { controller = "InventoryItems", action = "GetAll" }
            );

            config.Routes.MapHttpRoute(
                   name: "DefaultApi",
                   routeTemplate: "api/{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional }
               );

            //Tells the json formatter to preserve object references in the instance of circular references
            //var json = config.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            ////Removes XML formatter entirely, as I don't need it in this application.
            //config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
