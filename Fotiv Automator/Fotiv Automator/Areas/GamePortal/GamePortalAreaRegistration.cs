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

            context.MapRoute("Games",           "games",            new { controller = "Game", action = "Index" }, namespaces);
            context.MapRoute("NewGame",         "game/new",         new { controller = "Game", action = "New" }, namespaces);
            context.MapRoute("GameSettings",    "game/settings",    new { controller = "Game", action = "Edit" }, namespaces);
            context.MapRoute("Game",            "game/{gameID}",             new { controller = "Game", action = "Show" }, namespaces);

            #region Star Map
            context.MapRoute("StarMap",     "game/{gameID}/starmap",                 new { controller = "StarMap", action = "Show" }, namespaces);
            context.MapRoute("NewStarMap",  "game/{gameID}/starmap/new",             new { controller = "StarMap", action = "NewSector" }, namespaces);
            context.MapRoute("EditStarMap", "game/{gameID}/starmap/edit/{sectorID}", new { controller = "StarMap", action = "EditSector" }, namespaces);

            context.MapRoute("ViewStarsystem", "game/{gameID}/starmap/starsystem/{starsystemID}",        new { controller = "Starsystem", action = "Show" }, namespaces);
            context.MapRoute("EditStarsystem", "game/{gameID}/starmap/starsystem/edit/{starsystemID}",   new { controller = "Starsystem", action = "Edit" }, namespaces);

            context.MapRoute("NewStar",   "game/{gameID}/starmap/star/new",              new { controller = "Star", action = "New" }, namespaces);
            context.MapRoute("ViewStar",  "game/{gameID}/starmap/star/{starID}",         new { controller = "Star", action = "Show" }, namespaces);
            context.MapRoute("EditStar",  "game/{gameID}/starmap/star/edit/{starID}",    new { controller = "Star", action = "Edit" }, namespaces);

            context.MapRoute("NewWormhole",   "game/{gameID}/starmap/wormhole/new",                  new { controller = "Wormhole", action = "New" }, namespaces);
            context.MapRoute("ViewWormhole",  "game/{gameID}/starmap/wormhole/{wormholeID}",         new { controller = "Wormhole", action = "Show" }, namespaces);
            context.MapRoute("EditWormhole",  "game/{gameID}/starmap/wormhole/edit/{wormholeID}",    new { controller = "Wormhole", action = "Edit" }, namespaces);

            context.MapRoute("NewPlanet",           "game/{gameID}/starmap/planet/new",              new { controller = "Planet", action = "NewPlanet" }, namespaces);
            context.MapRoute("ViewPlanet",          "game/{gameID}/starmap/planet/{planetID}",       new { controller = "Planet", action = "Show" }, namespaces);
            context.MapRoute("EditPlanet",          "game/{gameID}/starmap/planet/edit/{planetID}",  new { controller = "Planet", action = "Edit" }, namespaces);
            #endregion

            #region Civilization Based
            context.MapRoute("Civilizations",       "game/{gameID}/civilizations",                           new { controller = "Civilization", action = "Index" }, namespaces);
            context.MapRoute("NewCivilization",     "game/{gameID}/civilizations/new",                       new { controller = "Civilization", action = "New" }, namespaces);
            context.MapRoute("ViewCivilization",    "game/{gameID}/civilizations/{civilizationID}",          new { controller = "Civilization", action = "Show" }, namespaces);
            context.MapRoute("EditCivilization",    "game/{gameID}/civilizations/edit/{civilizationID}",     new { controller = "Civilization", action = "Edit" }, namespaces);

            context.MapRoute("NewBattlegroup",     "game/{gameID}/civilizations/{civilizationID}/Battlegroups/new",                   new { controller = "Battlegroup", action = "New" }, namespaces);
            context.MapRoute("ViewBattlegroup",    "game/{gameID}/civilizations/{civilizationID}/Battlegroups/{battlegroupID}",       new { controller = "Battlegroup", action = "Show" }, namespaces);
            context.MapRoute("EditBattlegroup",    "game/{gameID}/civilizations/{civilizationID}/Battlegroups/edit/{battlegroupID}",  new { controller = "Battlegroup", action = "Edit" }, namespaces);

            #region Assets
            context.MapRoute("NewCivilizationResearch",  "game/{gameID}/civilizations/{civilizationID}/Assets/Research/new",                              new { controller = "CivilizationResearch", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationResearch", "game/{gameID}/civilizations/{civilizationID}/Assets/Research/{civilizationResearchID}",         new { controller = "CivilizationResearch", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationResearch", "game/{gameID}/civilizations/{civilizationID}/Assets/Research/edit/{civilizationResearchID}",    new { controller = "CivilizationResearch", action = "Edit" }, namespaces);
            
            context.MapRoute("NewCivilizationUnit",     "game/{gameID}/civilizations/{civilizationID}/Assets/Units/new",                        new { controller = "CivilizationUnit", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationUnit",    "game/{gameID}/civilizations/{civilizationID}/Assets/Units/{civilizationUnitID}",       new { controller = "CivilizationUnit", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationUnit",    "game/{gameID}/civilizations/{civilizationID}/Assets/Units/edit/{civilizationUnitID}",  new { controller = "CivilizationUnit", action = "Edit" }, namespaces);

            context.MapRoute("NewCivilizationInfrastructure",  "game/{gameID}/civilizations/{civilizationID}/Assets/Infrastructure/new",                                 new { controller = "CivilizationInfrastructure", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationInfrastructure", "game/{gameID}/civilizations/{civilizationID}/Assets/Infrastructure/{civilizationInfrastructureID}",      new { controller = "CivilizationInfrastructure", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationInfrastructure", "game/{gameID}/civilizations/{civilizationID}/Assets/Infrastructure/edit/{civilizationInfrastructureID}", new { controller = "CivilizationInfrastructure", action = "Edit" }, namespaces);
            #endregion

            #region R&D
            context.MapRoute("NewRnDResearch",  "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/Research/new",                    new { controller = "CivilizationRnDResearch", action = "New" }, namespaces);
            context.MapRoute("ViewRnDResearch", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/Research/{rndResearchID}",        new { controller = "CivilizationRnDResearch", action = "Show" }, namespaces);
            context.MapRoute("EditRnDResearch", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/Research/edit/{rndResearchID}",   new { controller = "CivilizationRnDResearch", action = "Edit" }, namespaces);

            context.MapRoute("NewRnDShip",  "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/ShipConstruction/new",               new { controller = "CivilizationRnDUnit", action = "NewShip" }, namespaces);
            context.MapRoute("EditRnDShip", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/ShipConstruction/edit/{rndUnitID}",  new { controller = "CivilizationRnDUnit", action = "EditShip" }, namespaces);
            context.MapRoute("NewRnDUnit",  "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/UnitTraining/new",                   new { controller = "CivilizationRnDUnit", action = "NewUnit" }, namespaces);
            context.MapRoute("EditRnDUnit", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/UnitTraining/edit/{rndUnitID}",      new { controller = "CivilizationRnDUnit", action = "EditUnit" }, namespaces);
            context.MapRoute("ViewRnDUnit", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/UnitTraining/{rndUnitID}",           new { controller = "CivilizationRnDUnit", action = "Show" }, namespaces);

            context.MapRoute("NewRnDColonialDevelopment",  "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/ColonialDevelopment/new",                                 new { controller = "CivilizationRnDColonialDevelopment", action = "New" }, namespaces);
            context.MapRoute("ViewRnDColonialDevelopment", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/ColonialDevelopment/{rndColonialDevelopmentID}",          new { controller = "CivilizationRnDColonialDevelopment", action = "Show" }, namespaces);
            context.MapRoute("EditRnDColonialDevelopment", "game/{gameID}/civilizations/{civilizationID}/ResearchAndDevelopment/ColonialDevelopment/edit/{rndColonialDevelopmentID}",     new { controller = "CivilizationRnDColonialDevelopment", action = "Edit" }, namespaces);
            #endregion
            #endregion

            #region Statistics
            context.MapRoute("Statistics", "game/{gameID}/statistics", new { controller = "Statistics", action = "Index" }, namespaces);

            context.MapRoute("Species",     "game/{gameID}/statistics/species",                      new { controller = "Species", action = "Index" }, namespaces);
            context.MapRoute("NewSpecies",  "game/{gameID}/statistics/species/new",                  new { controller = "Species", action = "New" }, namespaces);
            context.MapRoute("ViewSpecies", "game/{gameID}/statistics/species/{speciesID}",          new { controller = "Species", action = "Show" }, namespaces);
            context.MapRoute("EditSpecies", "game/{gameID}/statistics/species/edit/{speciesID}",     new { controller = "Species", action = "Edit" }, namespaces);

            context.MapRoute("Research",        "game/{gameID}/statistics/research",                     new { controller = "Research", action = "Index" }, namespaces);
            context.MapRoute("NewResearch",     "game/{gameID}/statistics/research/new",                 new { controller = "Research", action = "New" }, namespaces);
            context.MapRoute("ViewResearch",    "game/{gameID}/statistics/research/{researchID}",        new { controller = "Research", action = "Show" }, namespaces);
            context.MapRoute("EditResearch",    "game/{gameID}/statistics/research/edit/{researchID}",   new { controller = "Research", action = "Edit" }, namespaces);

            context.MapRoute("Units",       "game/{gameID}/statistics/units",                new { controller = "Unit", action = "Index" }, namespaces);
            context.MapRoute("NewUnit",     "game/{gameID}/statistics/units/new",            new { controller = "Unit", action = "New" }, namespaces);
            context.MapRoute("ViewUnit",    "game/{gameID}/statistics/units/{unitID}",       new { controller = "Unit", action = "Show" }, namespaces);
            context.MapRoute("EditUnit",    "game/{gameID}/statistics/units/edit/{unitID}",  new { controller = "Unit", action = "Edit" }, namespaces);

            context.MapRoute("Infrastructure",      "game/{gameID}/statistics/infrastructure",                           new { controller = "Infrastructure", action = "Index" }, namespaces);
            context.MapRoute("NewInfrastructure",   "game/{gameID}/statistics/infrastructure/new",                       new { controller = "Infrastructure", action = "New" }, namespaces);
            context.MapRoute("ViewInfrastructure",  "game/{gameID}/statistics/infrastructure/{infrastructureID}",        new { controller = "Infrastructure", action = "Show" }, namespaces);
            context.MapRoute("EditInfrastructure",  "game/{gameID}/statistics/infrastructure/edit/{infrastructureID}",   new { controller = "Infrastructure", action = "Edit" }, namespaces);

            context.MapRoute("UnitCategories",      "game/{gameID}/statistics/unit-category",                        new { controller = "UnitCategory", action = "Index" }, namespaces);
            context.MapRoute("NewUnitCategory",     "game/{gameID}/statistics/unit-category/new",                    new { controller = "UnitCategory", action = "New" }, namespaces);
            context.MapRoute("ViewUnitCategory",    "game/{gameID}/statistics/unit-category/{unitCategoryID}",       new { controller = "UnitCategory", action = "Show" }, namespaces);
            context.MapRoute("EditUnitCategory",    "game/{gameID}/statistics/unit-category/edit/{unitCategoryID}",  new { controller = "UnitCategory", action = "Edit" }, namespaces);
            
            context.MapRoute("CivilizationTraits",      "game/{gameID}/statistics/civilization-traits",                              new { controller = "CivilizationTrait", action = "Index" }, namespaces);
            context.MapRoute("NewCivilizationTrait",    "game/{gameID}/statistics/civilization-traits/new",                          new { controller = "CivilizationTrait", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationTrait",   "game/{gameID}/statistics/civilization-traits/{civilizationTraitID}",        new { controller = "CivilizationTrait", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationTrait",   "game/{gameID}/statistics/civilization-traits/edit/{civilizationTraitID}",   new { controller = "CivilizationTrait", action = "Edit" }, namespaces);
            
            context.MapRoute("TechLevels",     "game/{gameID}/statistics/tech-levels",                       new { controller = "TechLevel", action = "Index" }, namespaces);
            context.MapRoute("NewTechLevel",   "game/{gameID}/statistics/tech-levels/new",                   new { controller = "TechLevel", action = "New" }, namespaces);
            context.MapRoute("ViewTechLevel",  "game/{gameID}/statistics/tech-levels/{techLevelID}",         new { controller = "TechLevel", action = "Show" }, namespaces);
            context.MapRoute("EditTechLevel",  "game/{gameID}/statistics/tech-levels/edit/{techLevelID}",    new { controller = "TechLevel", action = "Edit" }, namespaces);

            context.MapRoute("PlanetTiers",     "game/{gameID}/statistics/planet-tiers",                         new { controller = "PlanetTier", action = "Index" }, namespaces);
            context.MapRoute("NewPlanetTier",   "game/{gameID}/statistics/planet-tiers/new",                     new { controller = "PlanetTier", action = "New" }, namespaces);
            context.MapRoute("ViewPlanetTier",  "game/{gameID}/statistics/planet-tiers/{planetTierID}",          new { controller = "PlanetTier", action = "Show" }, namespaces);
            context.MapRoute("EditPlanetTier",  "game/{gameID}/statistics/planet-tiers/edit/{planetTierID}",     new { controller = "PlanetTier", action = "Edit" }, namespaces);

            context.MapRoute("PlanetTypes",     "game/{gameID}/statistics/planet-types",                     new { controller = "PlanetType", action = "Index" }, namespaces);
            context.MapRoute("NewPlanetType",   "game/{gameID}/statistics/planet-types/new",                 new { controller = "PlanetType", action = "New" }, namespaces);
            context.MapRoute("ViewPlanetType",  "game/{gameID}/statistics/planet-types/{planetTypeID}",      new { controller = "PlanetType", action = "Show" }, namespaces);
            context.MapRoute("EditPlanetType",  "game/{gameID}/statistics/planet-types/edit/{planetTypeID}", new { controller = "PlanetType", action = "Edit" }, namespaces);

            context.MapRoute("StarTypes",     "game/{gameID}/statistics/star-types",                     new { controller = "StarType", action = "Index" }, namespaces);
            context.MapRoute("NewStarType",   "game/{gameID}/statistics/star-types/new",                 new { controller = "StarType", action = "New" }, namespaces);
            context.MapRoute("ViewStarType",  "game/{gameID}/statistics/star-types/{starTypeID}",        new { controller = "StarType", action = "Show" }, namespaces);
            context.MapRoute("EditStarType",  "game/{gameID}/statistics/star-types/edit/{starTypeID}",   new { controller = "StarType", action = "Edit" }, namespaces);

            context.MapRoute("StarAges",     "game/{gameID}/statistics/star-ages",                   new { controller = "StarAge", action = "Index" }, namespaces);
            context.MapRoute("NewStarAge",   "game/{gameID}/statistics/star-ages/new",               new { controller = "StarAge", action = "New" }, namespaces);
            context.MapRoute("ViewStarAge",  "game/{gameID}/statistics/star-ages/{starAgeID}",       new { controller = "StarAge", action = "Show" }, namespaces);
            context.MapRoute("EditStarAge",  "game/{gameID}/statistics/star-ages/edit/{starAgeID}",  new { controller = "StarAge", action = "Edit" }, namespaces);

            context.MapRoute("RadiationLevels",     "game/{gameID}/statistics/radiation-levels",                         new { controller = "RadiationLevel", action = "Index" }, namespaces);
            context.MapRoute("NewRadiationLevel",   "game/{gameID}/statistics/radiation-levels/new",                     new { controller = "RadiationLevel", action = "New" }, namespaces);
            context.MapRoute("ViewRadiationLevel",  "game/{gameID}/statistics/radiation-levels/{radiationLevelID}",      new { controller = "RadiationLevel", action = "Show" }, namespaces);
            context.MapRoute("EditRadiationLevel",  "game/{gameID}/statistics/radiation-levels/edit/{radiationLevelID}", new { controller = "RadiationLevel", action = "Edit" }, namespaces);

            context.MapRoute("StageOfLife",     "game/{gameID}/statistics/stage-of-life",                        new { controller = "StageOfLife", action = "Index" }, namespaces);
            context.MapRoute("NewStageOfLife",  "game/{gameID}/statistics/stage-of-life/new",                    new { controller = "StageOfLife", action = "New" }, namespaces);
            context.MapRoute("ViewStageOfLife", "game/{gameID}/statistics/stage-of-life/{stageOfLifeID}",        new { controller = "StageOfLife", action = "Show" }, namespaces);
            context.MapRoute("EditStageOfLife", "game/{gameID}/statistics/stage-of-life/edit/{stageOfLifeID}",   new { controller = "StageOfLife", action = "Edit" }, namespaces);

            context.MapRoute("ExperienceLevels",    "game/{gameID}/statistics/experience-levels",                            new { controller = "ExperienceLevel", action = "Index" }, namespaces);
            context.MapRoute("NewExperienceLevel",  "game/{gameID}/statistics/experience-levels/new",                        new { controller = "ExperienceLevel", action = "New" }, namespaces);
            context.MapRoute("ViewExperienceLevel", "game/{gameID}/statistics/experience-levels/{experienceLevelID}",        new { controller = "ExperienceLevel", action = "Show" }, namespaces);
            context.MapRoute("EditExperienceLevel", "game/{gameID}/statistics/experience-levels/edit/{experienceLevelID}",   new { controller = "ExperienceLevel", action = "Edit" }, namespaces);
            #endregion

            context.MapRoute(
                "GamePortal_default",
                "GamePortal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}