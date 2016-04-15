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
    public class StageOfLifeController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Stage of Life Controller: Index - gameID={GameState.GameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexStageOfLife
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StageOfLife = game.GameStatistics.StageOfLife
            });
        }

        [HttpGet]
        public override ActionResult Show(int? stageOfLifeID)
        {
            Debug.WriteLine($"GET: Stage of Life Controller: View - stageOfLifeID={stageOfLifeID}");
            if (stageOfLifeID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewStageOfLife
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                StageOfLife = game.GameStatistics.StageOfLife.Find(x => x.id == stageOfLifeID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine($"GET: Stage of Life Controller: New");
            return View(new StageOfLifeForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(StageOfLifeForm form)
        {
            Debug.WriteLine($"POST: Stage of Life Controller: New - gameID={GameState.GameID}");
            var game = GameState.Game;

            DB_stage_of_life stageOfLife = new DB_stage_of_life();
            stageOfLife.game_id = game.Info.id;
            stageOfLife.name = form.Name;
            Database.Session.Save(stageOfLife);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? stageOfLifeID)
        {
            Debug.WriteLine($"GET: Stage of Life Controller: Edit - stageOfLifeID={stageOfLifeID}");
            var game = GameState.Game;

            var stageOfLife = game.GameStatistics.StageOfLife.Find(x => x.id == stageOfLifeID);
            return View(new StageOfLifeForm
            {
                ID = stageOfLife.id,
                Name = stageOfLife.name,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StageOfLifeForm form, int? stageOfLifeID)
        {
            Debug.WriteLine($"POST: Stage of Life Controller: Edit - stageOfLifeID={stageOfLifeID}");
            var game = GameState.Game;

            var stageOfLife = game.GameStatistics.StageOfLife.Find(x => x.id == stageOfLifeID);
            if (stageOfLife.game_id == null || stageOfLife.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            stageOfLife.name = form.Name;
            Database.Session.Update(stageOfLife);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? stageOfLifeID)
        {
            Debug.WriteLine($"POST: Stage of Life Controller: Delete - stageOfLifeID={stageOfLifeID}");

            var stageOfLife = Database.Session.Load<DB_stage_of_life>(stageOfLifeID);
            if (stageOfLife == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (stageOfLife.game_id == null || stageOfLife.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(stageOfLife);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}