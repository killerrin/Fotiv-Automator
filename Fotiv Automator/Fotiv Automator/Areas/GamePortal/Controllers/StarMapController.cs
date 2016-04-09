﻿using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Infrastructure.CustomControllers;
using Fotiv_Automator.Infrastructure.Extensions;
using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.StarMapGenerator;
using Fotiv_Automator.Models.Tools;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            game.QueryAndConnectSector();
            if (game.Sector == null) return RedirectToRoute("NewStarMap");

            DB_users user = Auth.User;
            return View(new ViewStarMap
            {
                GameID = game.Info.id,
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Sector = game.Sector
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

            DB_sectors dbSector = new DB_sectors();
            dbSector.game_id = game.ID;
            dbSector.name = form.Name;
            dbSector.description = form.Description;
            dbSector.gmnotes = form.GMNotes;
            Database.Session.Save(dbSector);

            if (form.FileUpload == null || form.FileUpload.ContentLength == 0)
                GenerateSector(game, dbSector, form.Width, form.Height);
            else
                ParseSectorFile(game, dbSector, form.FileUpload);

            Database.Session.Flush(); 
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }

        private void GenerateSector(Game game, DB_sectors dbSector, int width, int height)
        {
            StarSectorGenerator generator = new StarSectorGenerator(width, height);
            var generatedSector = generator.Generate();
            foreach (var column in generatedSector.Sector)
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
                        dbStar.age = star.Age.ToString().SpaceUppercaseLetters();
                        dbStar.radiation_level = star.Radiation.ToString().SpaceUppercaseLetters();
                        dbStar.name = star.Classification.ToString().SpaceUppercaseLetters();
                        Database.Session.Save(dbStar);

                        foreach (var celestialBody in star.CelestialBodies)
                        {
                            // Create the Planet
                            DB_planets dbPlanet = new DB_planets();
                            dbPlanet.game_id = game.ID;
                            dbPlanet.star_id = dbStar.id;
                            dbPlanet.planet_tier_id = RetrievePlanetaryTier(celestialBody.TerraformingTier.ToString().SpaceUppercaseLetters()).id;
                            dbPlanet.resources = celestialBody.ResourceValue;
                            dbPlanet.supports_colonies = celestialBody.SupportsColonies;
                            dbPlanet.name = celestialBody.CelestialType.ToString().SpaceUppercaseLetters();
                            dbPlanet.stage_of_life = celestialBody.StageOfLife.ToString().SpaceUppercaseLetters();
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
                                dbSatellite.resources = satellite.ResourceValue;
                                dbSatellite.supports_colonies = satellite.SupportsColonies;
                                dbSatellite.name = satellite.CelestialType.ToString().SpaceUppercaseLetters();
                                dbSatellite.stage_of_life = satellite.StageOfLife.ToString().SpaceUppercaseLetters();
                                Database.Session.Save(dbSatellite);

                                foreach (var species in satellite.Sentients)
                                    CreateSpeciesAndCivilization(game, dbSatellite, species);
                            }
                        }
                    }
                }
            }
        }

        private void ParseSectorFile(Game game, DB_sectors dbSector, HttpPostedFileBase fileUpload)
        {
            throw new NotImplementedException();
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
            dbCivilization.colour = "yellow";
            dbCivilization.name = $"Civilization {game.Random.Next(int.MaxValue)}";
            dbCivilization.rp = 0;
            Database.Session.Save(dbCivilization);

            DB_species dbSpecies = new DB_species();
            dbSpecies.game_id = game.ID;
            dbSpecies.name = $"Species {game.Random.Next(int.MaxValue)}";
            dbSpecies.description = string.Join(",", species.Classifications.Select(x => x.ToString()));
            Database.Session.Save(dbSpecies);

            DB_civilization_species dbCivSpecies = new DB_civilization_species();
            dbCivSpecies.civilization_id = dbCivilization.id;
            dbCivSpecies.species_id = dbSpecies.id;
            Database.Session.Save(dbCivSpecies);

            DB_civilization_infrastructure dbInfrastructure = new DB_civilization_infrastructure();
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
                Width = game.Sector.MaxX,
                Height = game.Sector.MaxY
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
            return RedirectToRoute("game", new { gameID = game.Info.id });
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
            if (sector.game_id == null || sector.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(sector);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}