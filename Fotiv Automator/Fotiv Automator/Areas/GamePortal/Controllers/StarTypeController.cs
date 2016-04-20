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
    public class StarTypeController : DataController
    {
        [HttpGet]
        public  ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Star Type Controller: Index - gameID={GameState.GameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexStarTypes
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StarTypes = game.GameStatistics.StarTypes
            });
        }

        [HttpGet]
        public  ActionResult Show(int gameID, int? starTypeID)
        {
            Debug.WriteLine($"GET: Star Type Controller: View - starTypeID={starTypeID}");
            if (starTypeID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewStarType
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StarType = game.GameStatistics.StarTypes.Find(x => x.id == starTypeID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID)
        {
            Debug.WriteLine($"GET: Star Type Controller: New");
            return View(new StarTypeForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(StarTypeForm form)
        {
            Debug.WriteLine($"POST: Star Type Controller: New - gameID={GameState.GameID}");
            var game = GameState.Game;

            DB_star_types starType = new DB_star_types();
            starType.game_id = game.Info.id;
            starType.name = form.Name;
            Database.Session.Save(starType);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int? starTypeID)
        {
            Debug.WriteLine($"GET: Star Type Controller: Edit - starTypeID={starTypeID}");
            var game = GameState.Game;

            var starType = game.GameStatistics.StarTypes.Find(x => x.id == starTypeID);
            return View(new StarTypeForm
            {
                ID = starType.id,
                Name = starType.name,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StarTypeForm form)
        {
            Debug.WriteLine($"POST: Star Type Controller: Edit - starTypeID={form.ID}");
            var game = GameState.Game;

            var starType = game.GameStatistics.StarTypes.Find(x => x.id == form.ID);
            if (starType.game_id == null || starType.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            starType.name = form.Name;
            Database.Session.Update(starType);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public  ActionResult Delete(int gameID, int? starTypeID)
        {
            Debug.WriteLine($"POST: Star Type Controller: Delete - starTypeID={starTypeID}");

            var starType = Database.Session.Load<DB_star_types>(starTypeID);
            if (starType == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (starType.game_id == null || starType.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(starType);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}