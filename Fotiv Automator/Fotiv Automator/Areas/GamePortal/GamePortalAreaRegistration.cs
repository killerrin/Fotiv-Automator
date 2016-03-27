using Fotiv_Automator.Areas.GamePortal.Controllers;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal
{
    public class GamePortalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GamePortal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            var namespaces = new[] { typeof(GameController).Namespace };

            context.MapRoute("Game", "game",        new { controller = "Game", action = "Index" }, namespaces);
            context.MapRoute("NewGame", "game/new", new { controller = "Game", action = "NewGame" }, namespaces);

            context.MapRoute("GameSettings", "game/settings", new { controller = "Settings", action = "Index" }, namespaces);

            context.MapRoute("Civilizations", "game/civilizations",                             new { controller = "Civilization", action = "Index" }, namespaces);
            context.MapRoute("NewCivilization", "game/civilizations/new",                       new { controller = "Civilization", action = "NewCivilization" }, namespaces);
            context.MapRoute("ViewCivilization", "game/civilizations/{civilizationID}",         new { controller = "Civilization", action = "ViewCivilization" }, namespaces);
            context.MapRoute("EditCivilization", "game/civilizations/edit/{civilizationID}",    new { controller = "Civilization", action = "EditCivilization" }, namespaces);

            context.MapRoute("Statistics", "game/statistics", new { controller = "Statistics", action = "Index" }, namespaces);

            context.MapRoute("Research", "game/statistics/research",                        new { controller = "Research", action = "Index" }, namespaces);
            context.MapRoute("NewResearch", "game/statistics/research/new",                 new { controller = "Research", action = "NewResearch" }, namespaces);
            context.MapRoute("ViewResearch", "game/statistics/research/{researchID}",       new { controller = "Research", action = "ViewResearch" }, namespaces);
            context.MapRoute("EditResearch", "game/statistics/research/edit/{researchID}",  new { controller = "Research", action = "EditResearch" }, namespaces);

            context.MapRoute("Ships", "game/statistics/ships",                  new { controller = "Ships", action = "Index" }, namespaces);
            context.MapRoute("NewShip", "game/statistics/ships/new",            new { controller = "Ships", action = "NewShip" }, namespaces);
            context.MapRoute("ViewShip", "game/statistics/ships/{shipID}",      new { controller = "Ships", action = "ViewShip" }, namespaces);
            context.MapRoute("EditShip", "game/statistics/ships/edit/{shipID}", new { controller = "Ships", action = "EditShip" }, namespaces);

            context.MapRoute("Infrastructure", "game/statistics/infrastructure",                                new { controller = "Infrastructure", action = "Index" }, namespaces);
            context.MapRoute("NewInfrastructure", "game/statistics/infrastructure/new",                         new { controller = "Infrastructure", action = "NewInfrastructure" }, namespaces);
            context.MapRoute("ViewInfrastructure", "game/statistics/infrastructure/{infrastructureID}",         new { controller = "Infrastructure", action = "ViewInfrastructure" }, namespaces);
            context.MapRoute("EditInfrastructure", "game/statistics/infrastructure/edit/{infrastructureID}",    new { controller = "Infrastructure", action = "EditInfrastructure" }, namespaces);

            context.MapRoute(
                "GamePortal_default",
                "GamePortal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}