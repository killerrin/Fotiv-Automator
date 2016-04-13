using Fotiv_Automator.Areas.GamePortal;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Areas.GamePortal.Models.Game;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fotiv_Automator.Infrastructure.CustomControllers;
using Fotiv_Automator.Infrastructure.Attributes;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class PlanetController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? planetID)
        {
            Debug.WriteLine($"GET: Planet Controller: View - planetID={planetID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            var planet = game.Sector.PlanetFromID(planetID.Value);
            Debug.WriteLine($"TotalSatellites: {planet.Satellites.Count}");

            return View(new ViewPlanet
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Planet = planet
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public ActionResult NewPlanet(int? starID = null, int? orbitingPlanetID = null)
        {
            Debug.WriteLine($"GET: Planet Controller: New Planet - starID={starID} orbitingPlanet={orbitingPlanetID}");
            var game = GameState.Game;

            var planetTiers = new List<Checkbox>();
            planetTiers.Add(new Checkbox(-1, "None", true));
            foreach (var tier in game.GameStatistics.PlanetTiers)
                planetTiers.Add(new Checkbox(tier.id, tier.name, false));

            var planetTypes = new List<Checkbox>();
            planetTypes.Add(new Checkbox(-1, "None", true));
            foreach (var type in game.GameStatistics.PlanetTypes)
                planetTypes.Add(new Checkbox(type.id, type.name, false));

            var stagesOfLife = new List<Checkbox>();
            stagesOfLife.Add(new Checkbox(-1, "None", true));
            foreach (var stageOfLife in game.GameStatistics.StageOfLife)
                stagesOfLife.Add(new Checkbox(stageOfLife.id, stageOfLife.name, false));

            return View(new PlanetForm
            {
                StarID = starID.Value,
                OrbitingPlanetID = orbitingPlanetID,

                PlanetTiers = planetTiers,
                PlanetTypes = planetTypes,
                StagesOfLife = stagesOfLife
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult NewPlanet(PlanetForm form, int? starID = null, int? orbitingPlanetID = null)
        {
            Debug.WriteLine($"POST: Planet Controller: New Planet- starID={form.StarID}");
            var game = GameState.Game;

            DB_planets planet = new DB_planets();
            planet.game_id = game.ID;
            planet.star_id = form.StarID;

            planet.orbiting_planet_id = (form.OrbitingPlanetID == -1) ? null : form.OrbitingPlanetID;
            planet.planet_tier_id = (form.SelectedPlanetTier == -1) ? null : form.SelectedPlanetTier;
            planet.planet_type_id = (form.SelectedPlanetType == -1) ? null : form.SelectedPlanetType;
            planet.stage_of_life_id = (form.SelectedStageOfLife == -1) ? null : form.SelectedStageOfLife;
            
            planet.name = form.Name;
            planet.resources = form.Resources;
            planet.supports_colonies = form.SupportsColonies;
            planet.gmnotes = form.GMNotes;
            Database.Session.Save(planet);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? planetID)
        {
            Debug.WriteLine($"GET: Planet Controller: Edit - planetID={planetID}");
            var game = GameState.Game;
            var planet = game.Sector.PlanetFromID(planetID.Value).Info;

            var planetTiers = new List<Checkbox>();
            planetTiers.Add(new Checkbox(-1, "None", false));
            foreach (var tier in game.GameStatistics.PlanetTiers)
                planetTiers.Add(new Checkbox(tier.id, tier.name, planet.planet_tier_id == tier.id));
            if (planetTiers.Where(x => x.IsChecked).ToList().Count == 0) planetTiers[0].IsChecked = true;

            var planetTypes = new List<Checkbox>();
            planetTypes.Add(new Checkbox(-1, "None", false));
            foreach (var type in game.GameStatistics.PlanetTypes)
                planetTypes.Add(new Checkbox(type.id, type.name, planet.planet_type_id == type.id));
            if (planetTypes.Where(x => x.IsChecked).ToList().Count == 0) planetTypes[0].IsChecked = true;

            var stagesOfLife = new List<Checkbox>();
            stagesOfLife.Add(new Checkbox(-1, "None", false));
            foreach (var stageOfLife in game.GameStatistics.StageOfLife)
                stagesOfLife.Add(new Checkbox(stageOfLife.id, stageOfLife.name, planet.stage_of_life_id == stageOfLife.id));
            if (stagesOfLife.Where(x => x.IsChecked).ToList().Count == 0) stagesOfLife[0].IsChecked = true;

            return View(new PlanetForm
            {
                ID = planet.id,
                StarID = planet.star_id,
                OrbitingPlanetID = planet.orbiting_planet_id,

                Name = planet.name,
                Resources = planet.resources,
                SupportsColonies = planet.supports_colonies,
                GMNotes = planet.gmnotes,

                PlanetTiers = planetTiers,
                PlanetTypes = planetTypes,
                StagesOfLife = stagesOfLife
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(PlanetForm form, int? planetID)
        {
            Debug.WriteLine($"POST: Planet Controller: Edit - planetID={planetID}");
            var game = GameState.Game;

            var planet = game.Sector.PlanetFromID(planetID.Value).Info;
            if (planet.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            planet.planet_tier_id = (form.SelectedPlanetTier == -1) ? null : form.SelectedPlanetTier;
            planet.planet_type_id = (form.SelectedPlanetType == -1) ? null : form.SelectedPlanetType;
            planet.stage_of_life_id = (form.SelectedStageOfLife == -1) ? null : form.SelectedStageOfLife;

            planet.name = form.Name;
            planet.resources = form.Resources;
            planet.supports_colonies = form.SupportsColonies;
            planet.gmnotes = form.GMNotes;
            Database.Session.Update(planet);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? planetID)
        {
            Debug.WriteLine($"POST: Planet Controller: Delete - planetID={planetID}");

            var planet = Database.Session.Load<DB_planets>(planetID.Value);
            if (planet == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (planet.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(planet);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}