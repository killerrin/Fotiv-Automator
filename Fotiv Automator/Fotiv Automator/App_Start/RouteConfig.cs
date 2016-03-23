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
            var namespaces = new[] { typeof(HomeController).Namespace };

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Auth", action = "Login" });
            routes.MapRoute("Home", "", new { controller = "Home", action = "Index" }, namespaces);

            routes.MapRoute("Login", "login", new { controller = "Auth", Action = "Login" }, namespaces);
            routes.MapRoute("Logout", "logout", new { controller = "Auth", Action = "Logout" }, namespaces);
            routes.MapRoute("CreateAccount", "createaccount", new { controller = "Auth", Action = "CreateAccount" }, namespaces);

            routes.MapRoute("AccountSettings", "settings", new { controller = "UserAccount", Action = "Index" }, namespaces);

            routes.MapRoute("Game", "game", new { controller = "Game", action = "Index" }, namespaces);
            routes.MapRoute("NewGame", "game/new", new { controller = "Game", action = "NewGame" }, namespaces);
            routes.MapRoute("GameSettings", "game/settings", new { controller = "Game", action = "GameSettings" }, namespaces);
            routes.MapRoute("Civilizations", "game/civilizations", new { controller = "Game", action = "Civilizations" }, namespaces);
            routes.MapRoute("NewCivilization", "game/civilizations/new", new { controller = "Game", action = "NewCivilization" }, namespaces);

            routes.MapRoute("Error500", "errors/500", new { controller = "Errors", action = "Error" }, namespaces);
            routes.MapRoute("Error404", "errors/404", new { controller = "Errors", action = "NotFound" }, namespaces);
        }
    }
}
