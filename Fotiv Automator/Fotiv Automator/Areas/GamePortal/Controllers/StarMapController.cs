using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Infrastructure.CustomControllers;
using Fotiv_Automator.Infrastructure.Extensions;
using Fotiv_Automator.Models;
using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.StarMapGenerator;
using Fotiv_Automator.Models.Tools;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class StarMapController : Controller
    {
        // GET: GamePortal/StarMap
        [HttpGet]
        public ActionResult Show()
        {
            Debug.WriteLine($"GET: Star Map Controller: Show Star Map");

            Game game = GameState.Game;
            game.GameStatistics.QueryPlanetTiers();
            game.GameStatistics.QuerySpecies();
            game.QueryAndConnectCivilizations();
            game.QueryAndConnectSector();
            if (game.Sector == null) return RedirectToRoute("NewStarMap");

            SafeUser user = Auth.User;

            List<Civilization> visibleCivilizations = new List<Civilization>();

            var playerCivilizations = game.Civilizations.Where(x => x.PlayerOwnsCivilization(user.ID)).ToList();
            visibleCivilizations.AddRange(playerCivilizations);

            foreach (var playerCivilization in playerCivilizations)
                visibleCivilizations.AddRange(playerCivilization.MetCivilizations);

            return View(new ViewStarMap
            {
                User = game.Players.Where(x => x.User.ID == user.ID).First(),
                Sector = game.Sector,
                VisibleCivilizations = visibleCivilizations.GroupBy(x => x.ID).Select(x => x.First()).ToList()
            });
        }

        [HttpPost]
        public ActionResult StarSystemDetails(int hexX, int hexY)
        {
            Debug.WriteLine($"StarSystemDetails HexX:{hexX} HexY:{hexY}");
            Game game = GameState.Game;

            if (hexX < 0 || hexY < 0 ||
                game == null || game.Sector == null)
                return PartialView("_Starsystem", new ViewStarSystem());

            SafeUser user = Auth.User;
            Starsystem system = game.Sector.StarsystemFromHex(new HexCoordinate(hexX, hexY));

            // Check to determine if the player is within a civilization that has visited the system
            var playerCivilizations = game.Civilizations
                .Where(x => x.PlayerOwnsCivilization(user.ID))
                .Where(x => x.HasVisitedSystem(system.ID))
                .ToList();

            if (!RequireGMAdminAttribute.IsGMOrAdmin() && playerCivilizations.Count == 0)
                return PartialView("_Starsystem", new ViewStarSystem());

            return PartialView("_Starsystem", new ViewStarSystem
            {
                User = game.Players.Where(x => x.User.ID == user.ID).First(),
                System = system,
                HeaderHotLink = true
            });
        }

        #region New Sector
        [HttpGet, RequireGMAdmin]
        public ActionResult NewSector()
        {
            Debug.WriteLine($"GET: Star Map Controller: New Sector");
            return View(new SectorForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult NewSector(SectorForm form)
        {
            Debug.WriteLine($"POST: Star Map Controller: New Sector");
            if (form.FileUpload == null || form.FileUpload.ContentLength == 0)
            {
                if (form.Width == 0)
                    ModelState.AddModelError("Width", "Width must be set to a value greater than 0");
                if (form.Height == 0)
                    ModelState.AddModelError("Height", "Height must be set to a value greater than 0");
            }
            else
            {
                if (form.FileUpload.ContentType != "text/plain")
                    ModelState.AddModelError("FileUpload", "Your file must be of format .txt");
            }

            if (!ModelState.IsValid)
                return View(form);

            var game = GameState.Game;

            Fotiv_Automator.Models.StarMapGenerator.Models.StarSector sector = null;
            if (form.FileUpload == null || form.FileUpload.ContentLength == 0)
            {
                Debug.WriteLine($"Star Map Controller: Generate Sector");
                StarSectorGenerator generator = new StarSectorGenerator(form.Width, form.Height);
                sector = generator.Generate();
            }
            else
            {
                Debug.WriteLine($"Star Map Controller: Load Sector From File");
                List<string> linesOfText = new List<string>();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(form.FileUpload.InputStream))
                {
                    while (!reader.EndOfStream)
                    {
                        linesOfText.Add(reader.ReadLine());
                    }
                }

                sector = StarSectorGenerator.Load(linesOfText);
            }

            if (sector == null || sector.Sector.Count == 0)
                Debug.WriteLine("Generation Failed: Sector Not Saved");
            else
            {
                DB_sectors dbSector = new DB_sectors();
                dbSector.game_id = game.ID;
                dbSector.name = form.Name;
                dbSector.description = form.Description;
                dbSector.gmnotes = form.GMNotes;
                Database.Session.Save(dbSector);

                SaveSector(game, dbSector, sector);
            }

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.ID });
        }

        private void SaveSector(Game game, DB_sectors dbSector, Fotiv_Automator.Models.StarMapGenerator.Models.StarSector sector)
        {
            Debug.WriteLine($"Star Map Controller: Saving Sector to DB");

            foreach (var column in sector.Sector)
            {
                foreach (var starsystem in column)
                {
                    DB_starsystems dbStarSystem = new DB_starsystems();
                    dbStarSystem.game_id = game.ID;
                    dbStarSystem.sector_id = dbSector.id;
                    dbStarSystem.hex_x = starsystem.Coordinate.X;
                    dbStarSystem.hex_y = starsystem.Coordinate.Y;
                    Database.Session.Save(dbStarSystem);

                    foreach (var star in starsystem.Stars)
                    {
                        DB_stars dbStar = new DB_stars();
                        dbStar.game_id = game.ID;
                        dbStar.starsystem_id = dbStarSystem.id;
                        dbStar.star_type_id = RetrieveStarType(star.Classification.ToString().SpaceUppercaseLetters()).id;
                        dbStar.star_age_id = RetrieveStarAge(star.Age.ToString().SpaceUppercaseLetters()).id;
                        dbStar.radiation_level_id = RetrieveRadiationLevel(star.Radiation.ToString().SpaceUppercaseLetters()).id;
                        dbStar.name = star.Classification.ToString().SpaceUppercaseLetters();
                        Database.Session.Save(dbStar);

                        foreach (var celestialBody in star.CelestialBodies)
                        {
                            // Create the Planet
                            DB_planets dbPlanet = new DB_planets();
                            dbPlanet.game_id = game.ID;
                            dbPlanet.star_id = dbStar.id;
                            dbPlanet.planet_tier_id = RetrievePlanetaryTier(celestialBody.TerraformingTier.ToString().SpaceUppercaseLetters()).id;
                            dbPlanet.planet_type_id = RetrievePlanetType(celestialBody.CelestialType.ToString().SpaceUppercaseLetters()).id;
                            dbPlanet.stage_of_life_id = RetrieveStageOfLife(celestialBody.StageOfLife.ToString().SpaceUppercaseLetters()).id;
                            dbPlanet.resources = celestialBody.ResourceValue;
                            dbPlanet.supports_colonies = celestialBody.SupportsColonies;
                            dbPlanet.name = celestialBody.CelestialType.ToString().SpaceUppercaseLetters();
                            Database.Session.Save(dbPlanet);

                            foreach (var species in celestialBody.Sentients)
                                CreateSpeciesAndCivilization(game, dbPlanet, species);

                            // And set up the satellites
                            foreach (var satellite in celestialBody.OrbitingSatellites)
                            {
                                DB_planets dbSatellite = new DB_planets();
                                dbSatellite.game_id = game.ID;
                                dbSatellite.star_id = dbStar.id;
                                dbSatellite.orbiting_planet_id = dbPlanet.id;
                                dbSatellite.planet_tier_id = RetrievePlanetaryTier(satellite.TerraformingTier.ToString().SpaceUppercaseLetters()).id;
                                dbSatellite.planet_type_id = RetrievePlanetType(satellite.CelestialType.ToString().SpaceUppercaseLetters()).id;
                                dbSatellite.stage_of_life_id = RetrieveStageOfLife(satellite.StageOfLife.ToString().SpaceUppercaseLetters()).id;
                                dbSatellite.resources = satellite.ResourceValue;
                                dbSatellite.supports_colonies = satellite.SupportsColonies;
                                dbSatellite.name = satellite.CelestialType.ToString().SpaceUppercaseLetters();
                                Database.Session.Save(dbSatellite);

                                foreach (var species in satellite.Sentients)
                                    CreateSpeciesAndCivilization(game, dbSatellite, species);
                            }
                        }
                    }
                }
            }
        }

        private DB_planet_tiers RetrievePlanetaryTier(string name)
        {
            var game = GameState.Game;

            var planetTier = Database.Session.Query<DB_planet_tiers>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (planetTier == null)
            {
                planetTier = new DB_planet_tiers();
                planetTier.game_id = game.ID;
                planetTier.build_rate = 10;
                planetTier.name = name;
                Database.Session.Save(planetTier);
            }

            return planetTier;
        }

        private DB_planet_types RetrievePlanetType(string name)
        {
            var game = GameState.Game;

            var planetType = Database.Session.Query<DB_planet_types>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (planetType == null)
            {
                planetType = new DB_planet_types();
                planetType.game_id = game.ID;
                planetType.name = name;
                Database.Session.Save(planetType);
            }

            return planetType;
        }

        private DB_stage_of_life RetrieveStageOfLife(string name)
        {
            var game = GameState.Game;

            var stageOfLife = Database.Session.Query<DB_stage_of_life>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (stageOfLife == null)
            {
                stageOfLife = new DB_stage_of_life();
                stageOfLife.game_id = game.ID;
                stageOfLife.name = name;
                Database.Session.Save(stageOfLife);
            }

            return stageOfLife;
        }

        private DB_star_types RetrieveStarType(string name)
        {
            var game = GameState.Game;

            var starType = Database.Session.Query<DB_star_types>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (starType == null)
            {
                starType = new DB_star_types();
                starType.game_id = game.ID;
                starType.name = name;
                Database.Session.Save(starType);
            }

            return starType;
        }

        private DB_star_ages RetrieveStarAge(string name)
        {
            var game = GameState.Game;

            var starAge = Database.Session.Query<DB_star_ages>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (starAge == null)
            {
                starAge = new DB_star_ages();
                starAge.game_id = game.ID;
                starAge.name = name;
                Database.Session.Save(starAge);
            }

            return starAge;
        }

        private DB_radiation_levels RetrieveRadiationLevel(string name)
        {
            var game = GameState.Game;

            var radiationLevel = Database.Session.Query<DB_radiation_levels>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (radiationLevel == null)
            {
                radiationLevel = new DB_radiation_levels();
                radiationLevel.game_id = game.ID;
                radiationLevel.name = name;
                Database.Session.Save(radiationLevel);
            }

            return radiationLevel;
        }

        private DB_civilization_traits RetrieveCivilizationTrait(string name)
        {
            var game = GameState.Game;

            var civilizationTrait = Database.Session.Query<DB_civilization_traits>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (civilizationTrait == null)
            {
                civilizationTrait = new DB_civilization_traits();
                civilizationTrait.game_id = game.ID;
                civilizationTrait.name = name;
                Database.Session.Save(civilizationTrait);
            }

            return civilizationTrait;
        }

        private DB_tech_levels RetrieveTechLevel(string name)
        {
            var game = GameState.Game;

            var techLevel = Database.Session.Query<DB_tech_levels>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (techLevel == null)
            {
                techLevel = new DB_tech_levels();
                techLevel.game_id = game.ID;
                techLevel.name = name;
                techLevel.attack_detriment = 0;
                Database.Session.Save(techLevel);
            }

            return techLevel;
        }

        private DB_infrastructure RetrieveInfrastructure(string name)
        {
            var game = GameState.Game;

            var infrastructure = Database.Session.Query<DB_infrastructure>()
                .Where(x => x.game_id == null || x.game_id == game.ID)
                .Where(x => x.name == name)
                .FirstOrDefault();

            if (infrastructure == null)
            {
                infrastructure = new DB_infrastructure();
                infrastructure.game_id = game.ID;
                infrastructure.name = name;
                infrastructure.is_colony = true;
                infrastructure.base_health = 1;
                Database.Session.Save(infrastructure);
            }

            return infrastructure;
        }

        private void CreateSpeciesAndCivilization(Game game, DB_planets planet, Fotiv_Automator.Models.StarMapGenerator.Models.SentientSpecies species)
        {
            // Create the Civilization
            DB_civilization dbCivilization = new DB_civilization();
            dbCivilization.game_id = game.ID;
            dbCivilization.civilization_traits_1_id = RetrieveCivilizationTrait(species.Traits[0].ToString().SpaceUppercaseLetters()).id;
            dbCivilization.civilization_traits_2_id = RetrieveCivilizationTrait(species.Traits[1].ToString().SpaceUppercaseLetters()).id;
            dbCivilization.civilization_traits_3_id = RetrieveCivilizationTrait(species.Traits[2].ToString().SpaceUppercaseLetters()).id;
            dbCivilization.tech_level_id = RetrieveTechLevel($"TL{(int)species.TechLevel} {species.TechLevel.ToString().SpaceUppercaseLetters()}").id;
            dbCivilization.colour = "#E4D12F";
            dbCivilization.name = $"Civilization {game.Random.Next(int.MaxValue)}";
            dbCivilization.rp = 0;
            Database.Session.Save(dbCivilization);

            DB_species dbSpecies = new DB_species();
            dbSpecies.game_id = game.ID;
            dbSpecies.name = $"Species {game.Random.Next(int.MaxValue)}";
            dbSpecies.description = string.Join(",", species.Classifications.Select(x => x.ToString()));
            Database.Session.Save(dbSpecies);

            DB_civilization_species dbCivSpecies = new DB_civilization_species();
            dbCivSpecies.game_id = game.ID;
            dbCivSpecies.civilization_id = dbCivilization.id;
            dbCivSpecies.species_id = dbSpecies.id;
            Database.Session.Save(dbCivSpecies);

            DB_civilization_infrastructure dbInfrastructure = new DB_civilization_infrastructure();
            dbInfrastructure.game_id = game.ID;
            dbInfrastructure.struct_id = RetrieveInfrastructure("Homeworld").id;
            dbInfrastructure.civilization_id = dbCivilization.id;
            dbInfrastructure.planet_id = planet.id;
            dbInfrastructure.build_percentage = 100;
            dbInfrastructure.name = "Homeworld";
            Database.Session.Save(dbInfrastructure);
        }
        #endregion

        #region Edit Sector
        [HttpGet, RequireGMAdmin]
        public ActionResult EditSector(int? sectorID)
        {
            Debug.WriteLine($"GET: Star Map Controller: Edit Sector");
            Game game = GameState.Game;

            return View(new SectorForm
            {
                ID = game.Sector.Info.id,
                Name = game.Sector.Info.name,
                Description = game.Sector.Info.description,
                GMNotes = game.Sector.Info.gmnotes,
                Width = game.Sector.Width,
                Height = game.Sector.Height
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult EditSector(SectorForm form, int? sectorID)
        {
            Debug.WriteLine($"POST: Star Map Controller: Edit Sector");
            var game = GameState.Game;

            DB_sectors dbSector = game.Sector.Info;
            dbSector.name = form.Name;
            dbSector.description = form.Description;
            dbSector.gmnotes = form.GMNotes;
            Database.Session.Update(dbSector);

            Database.Session.Flush();
            return RedirectToRoute("StarMap");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Delete(int? sectorID)
        {
            Debug.WriteLine($"POST: Star Map Controller: Delete Sector id={sectorID}");

            var sector = Database.Session.Load<DB_sectors>(sectorID);
            if (sector == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (sector.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(sector);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}