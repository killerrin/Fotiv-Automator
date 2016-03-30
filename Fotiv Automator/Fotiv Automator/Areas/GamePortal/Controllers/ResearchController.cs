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
    public class ResearchController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int researchID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Research Controller: Index - researchID={0}", researchID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = game.GameStatistics.Research
            });
        }

        [HttpGet]
        public override ActionResult View(int researchID = -1)
        {
            if (researchID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Research Controller: View - researchID={0}", researchID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = game.GameStatistics.Research.Find(x => x.id == researchID),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Research Controller: New"));
            return View(new ResearchForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(ResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_research research = new DB_research();
            research.game_id = game.Info.id;

            research.name = form.Name;
            research.description = form.Description;
            research.rp_cost = form.RPCost;

            research.apply_military = form.ApplyMilitary;
            research.apply_units = form.ApplyUnits;
            research.apply_ships = form.ApplyShips;
            research.apply_infrastructure = form.ApplyInfrastructure;

            research.health_bonus = form.HealthBonus;
            research.regeneration_bonus = form.RegenerationBonus;
            research.attack_bonus = form.AttackBonus;
            research.special_attack_bonus = form.SpecialAttackBonus;
            research.agility_bonus = form.AgilityBonus;

            research.science_bonus = form.ScienceBonus;
            research.colonial_development_bonus = form.ColonialDevelopmentBonus;
            research.ship_construction_bonus = form.ShipConstructionBonus;
            research.unit_training_bonus = form.UnitTrainingBonus;
            research.gmnotes = form.GMNotes;
            Database.Session.Save(research);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int researchID)
        {
            Debug.WriteLine(string.Format("GET: Research Controller: Edit - researchID={0}", researchID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var research = game.GameStatistics.Research.Find(x => x.id == researchID);
            return View(new ResearchForm
            {
                ID = research.id,

                Name = research.name,
                Description = research.description,
                RPCost = research.rp_cost,

                ApplyMilitary = research.apply_military,
                ApplyUnits = research.apply_units,
                ApplyShips = research.apply_ships,
                ApplyInfrastructure = research.apply_infrastructure,

                HealthBonus = research.health_bonus,
                RegenerationBonus = research.regeneration_bonus,
                AttackBonus = research.attack_bonus,
                SpecialAttackBonus = research.special_attack_bonus,
                AgilityBonus = research.agility_bonus,

                ScienceBonus = research.science_bonus,
                ColonialDevelopmentBonus = research.colonial_development_bonus,
                ShipConstructionBonus = research.ship_construction_bonus,
                UnitTrainingBonus = research.unit_training_bonus,

                GMNotes = research.gmnotes
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ResearchForm form, int researchID)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Edit - researchID={0}", researchID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            DB_research research = game.GameStatistics.Research.Find(x => x.id == researchID);
            if ((research.game_id == null || research.game_id != game.Info.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

            research.name = form.Name;
            research.description = form.Description;
            research.rp_cost = form.RPCost;

            research.apply_military = form.ApplyMilitary;
            research.apply_units = form.ApplyUnits;
            research.apply_ships = form.ApplyShips;
            research.apply_infrastructure = form.ApplyInfrastructure;

            research.health_bonus = form.HealthBonus;
            research.regeneration_bonus = form.RegenerationBonus;
            research.attack_bonus = form.AttackBonus;
            research.special_attack_bonus = form.SpecialAttackBonus;
            research.agility_bonus = form.AgilityBonus;

            research.science_bonus = form.ScienceBonus;
            research.colonial_development_bonus = form.ColonialDevelopmentBonus;
            research.ship_construction_bonus = form.ShipConstructionBonus;
            research.unit_training_bonus = form.UnitTrainingBonus;
            research.gmnotes = form.GMNotes;
            Database.Session.Update(research);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int researchID)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Delete - researchID={0}", researchID));

            var research = Database.Session.Load<DB_research>(researchID);
            if (research == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if ((research.game_id == null || research.game_id != game.Info.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(research);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}