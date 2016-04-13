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
    public class TechLevelController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? techLevelID = null)
        { 
            Debug.WriteLine(string.Format("GET: Tech Level Controller: Index - techLevelID={0}", techLevelID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexTechLevels
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                TechLevels = game.GameStatistics.TechLevels
            });
        }

        [HttpGet]
        public override ActionResult Show(int? techLevelID)
        {
            Debug.WriteLine(string.Format("GET: Tech Level Controller: View - techLevelID={0}", techLevelID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewTechLevel
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                TechLevel = game.GameStatistics.TechLevels.Find(x => x.id == techLevelID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Tech Level Controller: New"));
            return View(new TechLevelForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(TechLevelForm form)
        {
            Debug.WriteLine(string.Format("POST: Tech Level Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_tech_levels techLevel = new DB_tech_levels();
            techLevel.game_id = game.Info.id;
            techLevel.name = form.Name;
            techLevel.attack_detriment = form.AttackDetriment;
            Database.Session.Save(techLevel);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? techLevelID)
        {
            Debug.WriteLine(string.Format("GET: Tech Level Controller: Edit - techLevelID={0}", techLevelID));
            var game = GameState.Game;

            var techLevel = game.GameStatistics.TechLevels.Find(x => x.id == techLevelID);
            return View(new TechLevelForm
            {
                ID = techLevel.id,
                Name = techLevel.name,
                AttackDetriment = techLevel.attack_detriment
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(TechLevelForm form, int? techLevelID)
        {
            Debug.WriteLine(string.Format("POST: Tech Level Controller: Edit - techLevelID={0}", techLevelID));
            var game = GameState.Game;

            var techLevel = game.GameStatistics.TechLevels.Find(x => x.id == techLevelID);
            if (techLevel.game_id == null || techLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            techLevel.name = form.Name;
            techLevel.attack_detriment = form.AttackDetriment;
            Database.Session.Update(techLevel);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? techLevelID)
        {
            Debug.WriteLine(string.Format("POST: Tech Level Controller: Delete - techLevelID={0}", techLevelID));

            var techLevel = Database.Session.Load<DB_tech_levels>(techLevelID);
            if (techLevel == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (techLevel.game_id == null || techLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(techLevel);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}