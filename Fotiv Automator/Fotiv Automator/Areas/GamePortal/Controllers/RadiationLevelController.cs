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
    public class RadiationLevelController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Radiation Level Controller: Index - gameID={GameState.GameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexRadiationLevels
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                RadiationLevels = game.GameStatistics.Radiationlevels
            });
        }

        [HttpGet]
        public override ActionResult Show(int? radiationLevelID)
        {
            Debug.WriteLine($"GET: Radiation Level Controller: View - radiationLevelID={radiationLevelID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewRadiationLevel
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                RadiationLevel = game.GameStatistics.Radiationlevels.Find(x => x.id == radiationLevelID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine($"GET: Radiation Level Controller: New");
            return View(new RadiationLevelForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(RadiationLevelForm form)
        {
            Debug.WriteLine($"POST: Radiation Level Controller: New - gameID={GameState.GameID}");
            var game = GameState.Game;

            DB_radiation_levels radiationLevel = new DB_radiation_levels();
            radiationLevel.game_id = game.Info.id;
            radiationLevel.name = form.Name;
            Database.Session.Save(radiationLevel);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? radiationLevelID)
        {
            Debug.WriteLine($"GET: Radiation Level Controller: Edit - radiationLevelID={radiationLevelID}");
            var game = GameState.Game;

            var radiationLevel = game.GameStatistics.Radiationlevels.Find(x => x.id == radiationLevelID);
            return View(new RadiationLevelForm
            {
                ID = radiationLevel.id,
                Name = radiationLevel.name,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(RadiationLevelForm form, int? radiationLevelID)
        {
            Debug.WriteLine($"POST: Radiation Level Controller: Edit - radiationLevelID={radiationLevelID}");
            var game = GameState.Game;

            var radiationLevel = game.GameStatistics.Radiationlevels.Find(x => x.id == radiationLevelID);
            if (radiationLevel.game_id == null || radiationLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            radiationLevel.name = form.Name;
            Database.Session.Update(radiationLevel);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? radiationLevelID)
        {
            Debug.WriteLine($"POST: Radiation Level Controller: Delete - radiationLevelID={radiationLevelID}");

            var radiationLevel = Database.Session.Load<DB_radiation_levels>(radiationLevelID);
            if (radiationLevel == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (radiationLevel.game_id == null || radiationLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(radiationLevel);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}