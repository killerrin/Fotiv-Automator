﻿using Fotiv_Automator.Areas.GamePortal.Controllers;
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
            context.MapRoute("Statistics", "game/statistics", new { controller = "Statistics", action = "Index" }, namespaces);

            context.MapRoute("Civilizations", "game/civilizations",                             new { controller = "Civilization", action = "Index" }, namespaces);
            context.MapRoute("NewCivilization", "game/civilizations/new",                       new { controller = "Civilization", action = "New" }, namespaces);
            context.MapRoute("ViewCivilization", "game/civilizations/{civilizationID}",         new { controller = "Civilization", action = "View" }, namespaces);
            context.MapRoute("EditCivilization", "game/civilizations/edit/{civilizationID}",    new { controller = "Civilization", action = "Edit" }, namespaces);

            context.MapRoute("Research", "game/statistics/research",                        new { controller = "Research", action = "Index" }, namespaces);
            context.MapRoute("NewResearch", "game/statistics/research/new",                 new { controller = "Research", action = "New" }, namespaces);
            context.MapRoute("ViewResearch", "game/statistics/research/{researchID}",       new { controller = "Research", action = "View" }, namespaces);
            context.MapRoute("EditResearch", "game/statistics/research/edit/{researchID}",  new { controller = "Research", action = "Edit" }, namespaces);

            context.MapRoute("Ships", "game/statistics/ships",                  new { controller = "Ships", action = "Index" }, namespaces);
            context.MapRoute("NewShip", "game/statistics/ships/new",            new { controller = "Ships", action = "New" }, namespaces);
            context.MapRoute("ViewShip", "game/statistics/ships/{shipID}",      new { controller = "Ships", action = "View" }, namespaces);
            context.MapRoute("EditShip", "game/statistics/ships/edit/{shipID}", new { controller = "Ships", action = "Edit" }, namespaces);

            context.MapRoute("Infrastructure", "game/statistics/infrastructure",                                new { controller = "Infrastructure", action = "Index" }, namespaces);
            context.MapRoute("NewInfrastructure", "game/statistics/infrastructure/new",                         new { controller = "Infrastructure", action = "New" }, namespaces);
            context.MapRoute("ViewInfrastructure", "game/statistics/infrastructure/{infrastructureID}",         new { controller = "Infrastructure", action = "View" }, namespaces);
            context.MapRoute("EditInfrastructure", "game/statistics/infrastructure/edit/{infrastructureID}",    new { controller = "Infrastructure", action = "Edit" }, namespaces);

            context.MapRoute("ShipRates", "game/statistics/ship-rates",                      new { controller = "ShipRate", action = "Index" }, namespaces);
            context.MapRoute("NewShipRate", "game/statistics/ship-rates/new",                new { controller = "ShipRate", action = "New" }, namespaces);
            context.MapRoute("ViewShipRate", "game/statistics/ship-rates/{shipRateID}",      new { controller = "ShipRate", action = "View" }, namespaces);
            context.MapRoute("EditShipRate", "game/statistics/ship-rates/edit/{shipRateID}", new { controller = "ShipRate", action = "Edit" }, namespaces);

            context.MapRoute("PlanetTiers", "game/statistics/planet-tiers",                         new { controller = "PlanetTier", action = "Index" }, namespaces);
            context.MapRoute("NewPlanetTier", "game/statistics/planet-tiers/new",                   new { controller = "PlanetTier", action = "New" }, namespaces);
            context.MapRoute("ViewPlanetTier", "game/statistics/planet-tiers/{planetTierID}",       new { controller = "PlanetTier", action = "View" }, namespaces);
            context.MapRoute("EditPlanetTier", "game/statistics/planet-tiers/edit/{planetTierID}",  new { controller = "PlanetTier", action = "Edit" }, namespaces);


            context.MapRoute(
                "GamePortal_default",
                "GamePortal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}