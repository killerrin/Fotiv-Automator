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

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class PlanetTierController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int planetTierID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Index - planetTierID={0}", planetTierID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexPlanetaryTier
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetTiers = game.GameStatistics.PlanetTiers
            });
        }

        [HttpGet]
        public override ActionResult View(int planetTierID = -1)
        {
            if (planetTierID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: View - planetTierID={0}", planetTierID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewPlanetaryTier
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: New"));
            return View(new PlanetaryTierForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(PlanetaryTierForm form)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_planet_tiers planetTier = new DB_planet_tiers();
            planetTier.game_id = game.Info.id;
            planetTier.name = form.Name;
            planetTier.build_rate = form.BuildRate;
            Database.Session.Save(planetTier);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int planetTierID)
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Edit - planetTierID={0}", planetTierID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID);
            if (planetaryTier.game_id == null || planetaryTier.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            return View(new PlanetaryTierForm
            {
                ID = planetaryTier.id,
                Name = planetaryTier.name,
                BuildRate = planetaryTier.build_rate
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(PlanetaryTierForm form, int planetTierID)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: Edit - planetTierID={0}", planetTierID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetTierID);
            if (planetaryTier.game_id == null || planetaryTier.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            planetaryTier.name = form.Name;
            planetaryTier.build_rate = form.BuildRate;
            Database.Session.Update(planetaryTier);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int planetTierID)
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
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}