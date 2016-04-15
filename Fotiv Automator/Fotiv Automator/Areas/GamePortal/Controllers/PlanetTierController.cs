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
    public class PlanetTierController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? planetTierID = null)
        { 
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Index - planetTierID={0}", planetTierID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexPlanetTiers
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetTiers = game.GameStatistics.PlanetTiers
            });
        }

        [HttpGet]
        public override ActionResult Show(int? planetTierID)
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: View - planetTierID={0}", planetTierID));
            if (planetTierID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewPlanetTier
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: New"));
            return View(new PlanetTierForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(PlanetTierForm form)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_planet_tiers planetTier = new DB_planet_tiers();
            planetTier.game_id = game.Info.id;
            planetTier.name = form.Name;
            planetTier.build_rate = form.BuildRate;
            Database.Session.Save(planetTier);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? planetTierID)
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Edit - planetTierID={0}", planetTierID));
            var game = GameState.Game;

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID);
            return View(new PlanetTierForm
            {
                ID = planetaryTier.id,
                Name = planetaryTier.name,
                BuildRate = planetaryTier.build_rate
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(PlanetTierForm form, int? planetTierID)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: Edit - planetTierID={0}", planetTierID));
            var game = GameState.Game;

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID);
            if (planetaryTier.game_id == null || planetaryTier.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            planetaryTier.name = form.Name;
            planetaryTier.build_rate = form.BuildRate;
            Database.Session.Update(planetaryTier);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? planetTierID)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: Delete - planetTierID={0}", planetTierID));

            var planetaryTier = Database.Session.Load<DB_planet_tiers>(planetTierID);
            if (planetaryTier == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (planetaryTier.game_id == null || planetaryTier.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(planetaryTier);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}