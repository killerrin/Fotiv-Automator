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
    public class StarAgeController : DataController
    {
        [HttpGet]
        public  ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Star Age Controller: Index - gameID={GameState.GameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexStarAges
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StarAges = game.GameStatistics.StarAges
            });
        }

        [HttpGet]
        public  ActionResult Show(int gameID, int? starAgeID)
        {
            Debug.WriteLine($"GET: Star Age Controller: View - starAgeID={starAgeID}");
            if (starAgeID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewStarAge
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StarAge = game.GameStatistics.StarAges.Find(x => x.id == starAgeID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID)
        {
            Debug.WriteLine($"GET: Star Age Controller: New");
            return View(new StarAgeForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(StarAgeForm form)
        {
            Debug.WriteLine($"POST: Star Age Controller: New - gameID={GameState.GameID}");
            var game = GameState.Game;

            DB_star_ages starAge = new DB_star_ages();
            starAge.game_id = game.Info.id;
            starAge.name = form.Name;
            Database.Session.Save(starAge);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int? starAgeID)
        {
            Debug.WriteLine($"GET: Star Age Controller: Edit - starAgeID={starAgeID}");
            var game = GameState.Game;

            var starAge = game.GameStatistics.StarAges.Find(x => x.id == starAgeID);
            return View(new StarAgeForm
            {
                ID = starAge.id,
                Name = starAge.name,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StarAgeForm form)
        {
            Debug.WriteLine($"POST: Star Age Controller: Edit - starAgeID={form.ID}");
            var game = GameState.Game;

            var starAge = game.GameStatistics.StarAges.Find(x => x.id == form.ID);
            if (starAge.game_id == null || starAge.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            starAge.name = form.Name;
            Database.Session.Update(starAge);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public  ActionResult Delete(int gameID, int? starAgeID)
        {
            Debug.WriteLine($"POST: Star Age Controller: Delete - starAgeID={starAgeID}");

            var starAge = Database.Session.Load<DB_star_ages>(starAgeID);
            if (starAge == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (starAge.game_id == null || starAge.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(starAge);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}