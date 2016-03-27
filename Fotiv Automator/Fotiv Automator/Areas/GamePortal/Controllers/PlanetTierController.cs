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
        public override ActionResult Index(int planetaryTierID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Index - planetaryTierID={0}", planetaryTierID));

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
        public override ActionResult View(int planetaryTierID = -1)
        {
            if (planetaryTierID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: View - planetaryTierID={0}", planetaryTierID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewPlanetaryTier
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetaryTierID),
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
            planetTier.name = form.Name;
            planetTier.build_rate = form.BuildRate;
            Database.Session.Save(planetTier);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int planetaryTierID)
        {
            Debug.WriteLine(string.Format("GET: Planetary Tier Controller: Edit - planetaryTierID={0}", planetaryTierID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetaryTierID);
            return View(new PlanetaryTierForm
            {
                ID = planetaryTier.id,
                Name = planetaryTier.name,
                BuildRate = planetaryTier.build_rate
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(PlanetaryTierForm form, int planetaryTierID)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: Edit - planetaryTierID={0}", planetaryTierID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var planetaryTier = game.GameStatistics.PlanetTiers.Find(x => x.id == planetaryTierID);
            planetaryTier.name = form.Name;
            planetaryTier.build_rate = form.BuildRate;
            Database.Session.Update(planetaryTier);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int planetaryTierID)
        {
            Debug.WriteLine(string.Format("POST: Planetary Tier Controller: Delete - planetaryTierID={0}", planetaryTierID));

            var planetaryTier = Database.Session.Load<DB_planet_tiers>(planetaryTierID);
            if (planetaryTier == null)
                return HttpNotFound();

            Database.Session.Delete(planetaryTier);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}