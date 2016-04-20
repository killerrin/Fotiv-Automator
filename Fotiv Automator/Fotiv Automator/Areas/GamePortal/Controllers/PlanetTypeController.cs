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
    public class PlanetTypeController : DataController
    {
        [HttpGet]
        public  ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Planet Type Controller: Index - gameID={GameState.GameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexPlanetTypes
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetTypes = game.GameStatistics.PlanetTypes
            });
        }

        [HttpGet]
        public  ActionResult Show(int gameID, int? planetTypeID)
        {
            Debug.WriteLine($"GET: Planet Type Controller: View - planetTypeID={planetTypeID}");
            if (planetTypeID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewPlanetType
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                PlanetType = game.GameStatistics.PlanetTypes.Find(x => x.id == planetTypeID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID)
        {
            Debug.WriteLine($"GET: Planet Type Controller: New");
            return View(new PlanetTypeForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(PlanetTypeForm form)
        {
            Debug.WriteLine($"POST: Planet Type Controller: New - gameID={GameState.GameID}");
            var game = GameState.Game;

            DB_planet_types planetType = new DB_planet_types();
            planetType.game_id = game.Info.id;
            planetType.name = form.Name;
            Database.Session.Save(planetType);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int? planetTypeID)
        {
            Debug.WriteLine($"GET: Planet Type Controller: Edit - planetTypeID={planetTypeID}");
            var game = GameState.Game;

            var planetaryType = game.GameStatistics.PlanetTypes.Find(x => x.id == planetTypeID);
            return View(new PlanetTypeForm
            {
                ID = planetaryType.id,
                Name = planetaryType.name,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(PlanetTypeForm form)
        {
            Debug.WriteLine($"POST: Planet Type Controller: Edit - planetTypeID={form.ID}");
            var game = GameState.Game;

            var planetaryType = game.GameStatistics.PlanetTypes.Find(x => x.id == form.ID);
            if (planetaryType.game_id == null || planetaryType.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            planetaryType.name = form.Name;
            Database.Session.Update(planetaryType);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public  ActionResult Delete(int gameID, int? planetTypeID)
        {
            Debug.WriteLine($"POST: Planet Type Controller: Delete - planetTypeID={planetTypeID}");

            var planetaryType = Database.Session.Load<DB_planet_types>(planetTypeID);
            if (planetaryType == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (planetaryType.game_id == null || planetaryType.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(planetaryType);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}