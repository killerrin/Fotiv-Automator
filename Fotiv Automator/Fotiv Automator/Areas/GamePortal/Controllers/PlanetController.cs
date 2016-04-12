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
        public override ActionResult New(int? starID = null)
        {
            Debug.WriteLine($"GET: Planet Controller: New - starID={starID}");
            return View(new PlanetForm
            {
                StarID = starID.Value
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(PlanetForm form, int? starID = null)
        {
            Debug.WriteLine($"POST: Planet Controller: New - starID={form.StarID}");
            var game = GameState.Game;

            DB_planets planet = new DB_planets();
            planet.game_id = game.ID;
            planet.star_id = form.StarID;
            planet.name = form.Name;
            planet.stage_of_life = form.StageOfLife;
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

            return View(new PlanetForm
            {
                ID = planet.id,
                StarID = planet.star_id,
                Name = planet.name,
                StageOfLife = planet.stage_of_life,
                Resources = planet.resources,
                SupportsColonies = planet.supports_colonies,
                GMNotes = planet.gmnotes
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

            planet.name = form.Name;
            planet.stage_of_life = form.StageOfLife;
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