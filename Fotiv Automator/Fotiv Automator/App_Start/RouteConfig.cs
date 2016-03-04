using Fotiv_Automator.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fotiv_Automator.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Change typeof(controller) to Homepage controller
            var namespaces = new[] { typeof(ErrorsController).Namespace };

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Home", "", new { controller = "Errors", action = "Error" }, namespaces);


            routes.MapRoute("Error500", "errors/500", new { controller = "Errors", action = "Error" }, namespaces);
            routes.MapRoute("Error404", "errors/404", new { controller = "Errors", action = "NotFound" }, namespaces);
        }
    }
}
