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
    public class ExperienceLevelController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? experienceLevelID = null)
        { 
            Debug.WriteLine(string.Format("GET: Experience Level Controller: Index - experienceLevelID={0}", experienceLevelID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexExperienceLevels
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                ExperienceLevels = game.GameStatistics.ExperienceLevels
            });
        }

        [HttpGet]
        public override ActionResult Show(int? experienceLevelID)
        {
            Debug.WriteLine(string.Format("GET: Experience Level Controller: View - experienceLevelID={0}", experienceLevelID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewExperienceLevel
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                ExperienceLevel = game.GameStatistics.ExperienceLevels.Find(x => x.id == experienceLevelID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Experience Level Controller: New"));
            return View(new ExperienceLevelForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(ExperienceLevelForm form)
        {
            Debug.WriteLine(string.Format("POST: Experience Level Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_experience_levels experienceLevel = new DB_experience_levels();
            experienceLevel.game_id = game.Info.id;
            experienceLevel.name = form.Name;
            experienceLevel.threshold = form.Threshold;
            experienceLevel.health_bonus = form.HealthBonus;
            experienceLevel.regeneration_bonus = form.RegenerationBonus;
            experienceLevel.attack_bonus = form.AttackBonus;
            experienceLevel.special_attack_bonus = form.SpecialAttackBonus;
            experienceLevel.agility_bonus = form.AgilityBonus;
            Database.Session.Save(experienceLevel);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? experienceLevelID)
        {
            Debug.WriteLine(string.Format("GET: Experience Level Controller: Edit - experienceLevelID={0}", experienceLevelID));
            var game = GameState.Game;

            var experienceLevel = game.GameStatistics.ExperienceLevels.Find(x => x.id == experienceLevelID);
            return View(new ExperienceLevelForm
            {
                ID = experienceLevel.id,

                Name = experienceLevel.name,
                Threshold = experienceLevel.threshold,

                HealthBonus = experienceLevel.health_bonus,
                RegenerationBonus = experienceLevel.regeneration_bonus,
                AttackBonus = experienceLevel.attack_bonus,
                SpecialAttackBonus = experienceLevel.special_attack_bonus,
                AgilityBonus = experienceLevel.agility_bonus,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(ExperienceLevelForm form, int? experienceLevelID)
        {
            Debug.WriteLine(string.Format("POST: Experience Level Controller: Edit - experienceLevelID={0}", experienceLevelID));
            var game = GameState.Game;

            DB_experience_levels experienceLevel = game.GameStatistics.ExperienceLevels.Find(x => x.id == experienceLevelID);
            if (experienceLevel.game_id == null || experienceLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            experienceLevel.game_id = game.Info.id;
            experienceLevel.name = form.Name;
            experienceLevel.threshold = form.Threshold;
            experienceLevel.health_bonus = form.HealthBonus;
            experienceLevel.regeneration_bonus = form.RegenerationBonus;
            experienceLevel.attack_bonus = form.AttackBonus;
            experienceLevel.special_attack_bonus = form.SpecialAttackBonus;
            experienceLevel.agility_bonus = form.AgilityBonus;
            Database.Session.Update(experienceLevel);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? experienceLevelID)
        {
            Debug.WriteLine(string.Format("POST: Experience Level Controller: Delete - experienceLevelID={0}", experienceLevelID));

            var experienceLevel = Database.Session.Load<DB_experience_levels>(experienceLevelID);
            if (experienceLevel == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (experienceLevel.game_id == null || experienceLevel.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(experienceLevel);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}