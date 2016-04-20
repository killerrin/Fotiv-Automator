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
            context.MapRoute("Game",            "game",             new { controller = "Game", action = "Show" }, namespaces);

            #region Star Map
            context.MapRoute("StarMap",     "game/starmap",                 new { controller = "StarMap", action = "Show" }, namespaces);
            context.MapRoute("NewStarMap",  "game/starmap/new",             new { controller = "StarMap", action = "NewSector" }, namespaces);
            context.MapRoute("EditStarMap", "game/starmap/edit/{sectorID}", new { controller = "StarMap", action = "EditSector" }, namespaces);

            context.MapRoute("ViewStarsystem", "game/starmap/starsystem/{starsystemID}",        new { controller = "Starsystem", action = "Show" }, namespaces);
            context.MapRoute("EditStarsystem", "game/starmap/starsystem/edit/{starsystemID}",   new { controller = "Starsystem", action = "Edit" }, namespaces);

            context.MapRoute("NewStar",   "game/starmap/star/new",              new { controller = "Star", action = "New" }, namespaces);
            context.MapRoute("ViewStar",  "game/starmap/star/{starID}",         new { controller = "Star", action = "Show" }, namespaces);
            context.MapRoute("EditStar",  "game/starmap/star/edit/{starID}",    new { controller = "Star", action = "Edit" }, namespaces);

            context.MapRoute("NewWormhole",   "game/starmap/wormhole/new",                  new { controller = "Wormhole", action = "New" }, namespaces);
            context.MapRoute("ViewWormhole",  "game/starmap/wormhole/{wormholeID}",         new { controller = "Wormhole", action = "Show" }, namespaces);
            context.MapRoute("EditWormhole",  "game/starmap/wormhole/edit/{wormholeID}",    new { controller = "Wormhole", action = "Edit" }, namespaces);

            context.MapRoute("NewPlanet",           "game/starmap/planet/new",              new { controller = "Planet", action = "NewPlanet" }, namespaces);
            context.MapRoute("ViewPlanet",          "game/starmap/planet/{planetID}",       new { controller = "Planet", action = "Show" }, namespaces);
            context.MapRoute("EditPlanet",          "game/starmap/planet/edit/{planetID}",  new { controller = "Planet", action = "Edit" }, namespaces);
            #endregion

            #region Civilization Based
            context.MapRoute("Civilizations",       "game/civilizations",                           new { controller = "Civilization", action = "Index" }, namespaces);
            context.MapRoute("NewCivilization",     "game/civilizations/new",                       new { controller = "Civilization", action = "New" }, namespaces);
            context.MapRoute("ViewCivilization",    "game/civilizations/{civilizationID}",          new { controller = "Civilization", action = "Show" }, namespaces);
            context.MapRoute("EditCivilization",    "game/civilizations/edit/{civilizationID}",     new { controller = "Civilization", action = "Edit" }, namespaces);

            context.MapRoute("NewBattlegroup",     "game/civilizations/Battlegroups/new",                   new { controller = "Battlegroup", action = "New" }, namespaces);
            context.MapRoute("ViewBattlegroup",    "game/civilizations/Battlegroups/{battlegroupID}",       new { controller = "Battlegroup", action = "Show" }, namespaces);
            context.MapRoute("EditBattlegroup",    "game/civilizations/Battlegroups/edit/{battlegroupID}",  new { controller = "Battlegroup", action = "Edit" }, namespaces);

            #region Assets
            context.MapRoute("NewCivilizationResearch",  "game/civilizations/Assets/Research/new",                              new { controller = "CivilizationResearch", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationResearch", "game/civilizations/Assets/Research/{civilizationResearchID}",         new { controller = "CivilizationResearch", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationResearch", "game/civilizations/Assets/Research/edit/{civilizationResearchID}",    new { controller = "CivilizationResearch", action = "Edit" }, namespaces);
            
            context.MapRoute("NewCivilizationUnit",     "game/civilizations/Assets/Units/new",                        new { controller = "CivilizationUnit", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationUnit",    "game/civilizations/Assets/Units/{civilizationUnitID}",       new { controller = "CivilizationUnit", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationUnit",    "game/civilizations/Assets/Units/edit/{civilizationUnitID}",  new { controller = "CivilizationUnit", action = "Edit" }, namespaces);

            context.MapRoute("NewCivilizationInfrastructure",  "game/civilizations/Assets/Infrastructure/new",                                 new { controller = "CivilizationInfrastructure", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationInfrastructure", "game/civilizations/Assets/Infrastructure/{civilizationInfrastructureID}",      new { controller = "CivilizationInfrastructure", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationInfrastructure", "game/civilizations/Assets/Infrastructure/edit/{civilizationInfrastructureID}", new { controller = "CivilizationInfrastructure", action = "Edit" }, namespaces);
            #endregion

            #region R&D
            context.MapRoute("NewRnDResearch",  "game/civilizations/ResearchAndDevelopment/Research/new",                    new { controller = "CivilizationRnDResearch", action = "New" }, namespaces);
            context.MapRoute("ViewRnDResearch", "game/civilizations/ResearchAndDevelopment/Research/{rndResearchID}",        new { controller = "CivilizationRnDResearch", action = "Show" }, namespaces);
            context.MapRoute("EditRnDResearch", "game/civilizations/ResearchAndDevelopment/Research/edit/{rndResearchID}",   new { controller = "CivilizationRnDResearch", action = "Edit" }, namespaces);

            context.MapRoute("NewRnDShip",  "game/civilizations/ResearchAndDevelopment/ShipConstruction/new",               new { controller = "CivilizationRnDUnit", action = "NewShip" }, namespaces);
            context.MapRoute("EditRnDShip", "game/civilizations/ResearchAndDevelopment/ShipConstruction/edit/{rndUnitID}",  new { controller = "CivilizationRnDUnit", action = "EditShip" }, namespaces);
            context.MapRoute("NewRnDUnit",  "game/civilizations/ResearchAndDevelopment/UnitTraining/new",                   new { controller = "CivilizationRnDUnit", action = "NewUnit" }, namespaces);
            context.MapRoute("EditRnDUnit", "game/civilizations/ResearchAndDevelopment/UnitTraining/edit/{rndUnitID}",      new { controller = "CivilizationRnDUnit", action = "EditUnit" }, namespaces);
            context.MapRoute("ViewRnDUnit", "game/civilizations/ResearchAndDevelopment/UnitTraining/{rndUnitID}",           new { controller = "CivilizationRnDUnit", action = "Show" }, namespaces);

            context.MapRoute("NewRnDColonialDevelopment",  "game/civilizations/ResearchAndDevelopment/ColonialDevelopment/new",                                 new { controller = "CivilizationRnDColonialDevelopment", action = "New" }, namespaces);
            context.MapRoute("ViewRnDColonialDevelopment", "game/civilizations/ResearchAndDevelopment/ColonialDevelopment/{rndColonialDevelopmentID}",          new { controller = "CivilizationRnDColonialDevelopment", action = "Show" }, namespaces);
            context.MapRoute("EditRnDColonialDevelopment", "game/civilizations/ResearchAndDevelopment/ColonialDevelopment/edit/{rndColonialDevelopmentID}",     new { controller = "CivilizationRnDColonialDevelopment", action = "Edit" }, namespaces);
            #endregion
            #endregion

            #region Statistics
            context.MapRoute("Statistics", "game/statistics", new { controller = "Statistics", action = "Index" }, namespaces);

            context.MapRoute("Species",     "game/statistics/species",                      new { controller = "Species", action = "Index" }, namespaces);
            context.MapRoute("NewSpecies",  "game/statistics/species/new",                  new { controller = "Species", action = "New" }, namespaces);
            context.MapRoute("ViewSpecies", "game/statistics/species/{speciesID}",          new { controller = "Species", action = "Show" }, namespaces);
            context.MapRoute("EditSpecies", "game/statistics/species/edit/{speciesID}",     new { controller = "Species", action = "Edit" }, namespaces);

            context.MapRoute("Research",        "game/statistics/research",                     new { controller = "Research", action = "Index" }, namespaces);
            context.MapRoute("NewResearch",     "game/statistics/research/new",                 new { controller = "Research", action = "New" }, namespaces);
            context.MapRoute("ViewResearch",    "game/statistics/research/{researchID}",        new { controller = "Research", action = "Show" }, namespaces);
            context.MapRoute("EditResearch",    "game/statistics/research/edit/{researchID}",   new { controller = "Research", action = "Edit" }, namespaces);

            context.MapRoute("Units",       "game/statistics/units",                new { controller = "Unit", action = "Index" }, namespaces);
            context.MapRoute("NewUnit",     "game/statistics/units/new",            new { controller = "Unit", action = "New" }, namespaces);
            context.MapRoute("ViewUnit",    "game/statistics/units/{unitID}",       new { controller = "Unit", action = "Show" }, namespaces);
            context.MapRoute("EditUnit",    "game/statistics/units/edit/{unitID}",  new { controller = "Unit", action = "Edit" }, namespaces);

            context.MapRoute("Infrastructure",      "game/statistics/infrastructure",                           new { controller = "Infrastructure", action = "Index" }, namespaces);
            context.MapRoute("NewInfrastructure",   "game/statistics/infrastructure/new",                       new { controller = "Infrastructure", action = "New" }, namespaces);
            context.MapRoute("ViewInfrastructure",  "game/statistics/infrastructure/{infrastructureID}",        new { controller = "Infrastructure", action = "Show" }, namespaces);
            context.MapRoute("EditInfrastructure",  "game/statistics/infrastructure/edit/{infrastructureID}",   new { controller = "Infrastructure", action = "Edit" }, namespaces);

            context.MapRoute("UnitCategories",          "game/statistics/unit-category",                        new { controller = "UnitCategory", action = "Index" }, namespaces);
            context.MapRoute("NewUnitCategory",         "game/statistics/unit-category/new",                    new { controller = "UnitCategory", action = "New" }, namespaces);
            context.MapRoute("ViewUnitCategory",     "game/statistics/unit-category/{unitCategoryID}",       new { controller = "UnitCategory", action = "Show" }, namespaces);
            context.MapRoute("EditUnitCategory",     "game/statistics/unit-category/edit/{unitCategoryID}",  new { controller = "UnitCategory", action = "Edit" }, namespaces);
            
            context.MapRoute("CivilizationTraits",      "game/statistics/civilization-traits",                              new { controller = "CivilizationTrait", action = "Index" }, namespaces);
            context.MapRoute("NewCivilizationTrait",    "game/statistics/civilization-traits/new",                          new { controller = "CivilizationTrait", action = "New" }, namespaces);
            context.MapRoute("ViewCivilizationTrait",   "game/statistics/civilization-traits/{civilizationTraitID}",        new { controller = "CivilizationTrait", action = "Show" }, namespaces);
            context.MapRoute("EditCivilizationTrait",   "game/statistics/civilization-traits/edit/{civilizationTraitID}",   new { controller = "CivilizationTrait", action = "Edit" }, namespaces);
            
            context.MapRoute("TechLevels",     "game/statistics/tech-levels",                       new { controller = "TechLevel", action = "Index" }, namespaces);
            context.MapRoute("NewTechLevel",   "game/statistics/tech-levels/new",                   new { controller = "TechLevel", action = "New" }, namespaces);
            context.MapRoute("ViewTechLevel",  "game/statistics/tech-levels/{techLevelID}",         new { controller = "TechLevel", action = "Show" }, namespaces);
            context.MapRoute("EditTechLevel",  "game/statistics/tech-levels/edit/{techLevelID}",    new { controller = "TechLevel", action = "Edit" }, namespaces);

            context.MapRoute("PlanetTiers",     "game/statistics/planet-tiers",                         new { controller = "PlanetTier", action = "Index" }, namespaces);
            context.MapRoute("NewPlanetTier",   "game/statistics/planet-tiers/new",                     new { controller = "PlanetTier", action = "New" }, namespaces);
            context.MapRoute("ViewPlanetTier",  "game/statistics/planet-tiers/{planetTierID}",          new { controller = "PlanetTier", action = "Show" }, namespaces);
            context.MapRoute("EditPlanetTier",  "game/statistics/planet-tiers/edit/{planetTierID}",     new { controller = "PlanetTier", action = "Edit" }, namespaces);

            context.MapRoute("PlanetTypes",     "game/statistics/planet-types",                     new { controller = "PlanetType", action = "Index" }, namespaces);
            context.MapRoute("NewPlanetType",   "game/statistics/planet-types/new",                 new { controller = "PlanetType", action = "New" }, namespaces);
            context.MapRoute("ViewPlanetType",  "game/statistics/planet-types/{planetTypeID}",      new { controller = "PlanetType", action = "Show" }, namespaces);
            context.MapRoute("EditPlanetType",  "game/statistics/planet-types/edit/{planetTypeID}", new { controller = "PlanetType", action = "Edit" }, namespaces);

            context.MapRoute("StarTypes",     "game/statistics/star-types",                     new { controller = "StarType", action = "Index" }, namespaces);
            context.MapRoute("NewStarType",   "game/statistics/star-types/new",                 new { controller = "StarType", action = "New" }, namespaces);
            context.MapRoute("ViewStarType",  "game/statistics/star-types/{starTypeID}",        new { controller = "StarType", action = "Show" }, namespaces);
            context.MapRoute("EditStarType",  "game/statistics/star-types/edit/{starTypeID}",   new { controller = "StarType", action = "Edit" }, namespaces);

            context.MapRoute("StarAges",     "game/statistics/star-ages",                   new { controller = "StarAge", action = "Index" }, namespaces);
            context.MapRoute("NewStarAge",   "game/statistics/star-ages/new",               new { controller = "StarAge", action = "New" }, namespaces);
            context.MapRoute("ViewStarAge",  "game/statistics/star-ages/{starAgeID}",       new { controller = "StarAge", action = "Show" }, namespaces);
            context.MapRoute("EditStarAge",  "game/statistics/star-ages/edit/{starAgeID}",  new { controller = "StarAge", action = "Edit" }, namespaces);

            context.MapRoute("RadiationLevels",     "game/statistics/radiation-levels",                         new { controller = "RadiationLevel", action = "Index" }, namespaces);
            context.MapRoute("NewRadiationLevel",   "game/statistics/radiation-levels/new",                     new { controller = "RadiationLevel", action = "New" }, namespaces);
            context.MapRoute("ViewRadiationLevel",  "game/statistics/radiation-levels/{radiationLevelID}",      new { controller = "RadiationLevel", action = "Show" }, namespaces);
            context.MapRoute("EditRadiationLevel",  "game/statistics/radiation-levels/edit/{radiationLevelID}", new { controller = "RadiationLevel", action = "Edit" }, namespaces);

            context.MapRoute("StageOfLife",     "game/statistics/stage-of-life",                        new { controller = "StageOfLife", action = "Index" }, namespaces);
            context.MapRoute("NewStageOfLife",  "game/statistics/stage-of-life/new",                    new { controller = "StageOfLife", action = "New" }, namespaces);
            context.MapRoute("ViewStageOfLife", "game/statistics/stage-of-life/{stageOfLifeID}",        new { controller = "StageOfLife", action = "Show" }, namespaces);
            context.MapRoute("EditStageOfLife", "game/statistics/stage-of-life/edit/{stageOfLifeID}",   new { controller = "StageOfLife", action = "Edit" }, namespaces);

            context.MapRoute("ExperienceLevels",    "game/statistics/experience-levels",                            new { controller = "ExperienceLevel", action = "Index" }, namespaces);
            context.MapRoute("NewExperienceLevel",  "game/statistics/experience-levels/new",                        new { controller = "ExperienceLevel", action = "New" }, namespaces);
            context.MapRoute("ViewExperienceLevel", "game/statistics/experience-levels/{experienceLevelID}",        new { controller = "ExperienceLevel", action = "Show" }, namespaces);
            context.MapRoute("EditExperienceLevel", "game/statistics/experience-levels/edit/{experienceLevelID}",   new { controller = "ExperienceLevel", action = "Edit" }, namespaces);
            #endregion

            context.MapRoute(
                "GamePortal_default",
                "GamePortal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}