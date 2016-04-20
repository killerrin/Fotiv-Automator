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
    public class ResearchController : DataController
    {
        [HttpGet]
        public  ActionResult Index(int gameID, int? researchID = null)
        { 
            Debug.WriteLine(string.Format("GET: Research Controller: Index - researchID={0}", researchID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = game.GameStatistics.Research
            });
        }

        [HttpGet]
        public  ActionResult Show(int gameID, int? researchID)
        {
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
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID)
        {
            Debug.WriteLine(string.Format("GET: Research Controller: New"));
            return View(new ResearchForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(ResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: New - gameID={0}", GameState.GameID));
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

            research.domestic_influence_bonus = form.DomesticInfluence;
            research.foreign_influence_bonus = form.ForeignInfluence;

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
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int? researchID)
        {
            Debug.WriteLine(string.Format("GET: Research Controller: Edit - researchID={0}", researchID));
            var game = GameState.Game;

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

                DomesticInfluence = research.domestic_influence_bonus,
                ForeignInfluence = research.foreign_influence_bonus,

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

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(ResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Edit - researchID={0}", form.ID));
            var game = GameState.Game;

            DB_research research = game.GameStatistics.Research.Find(x => x.id == form.ID);
            if (research.game_id == null || research.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            research.name = form.Name;
            research.description = form.Description;
            research.rp_cost = form.RPCost;

            research.apply_military = form.ApplyMilitary;
            research.apply_units = form.ApplyUnits;
            research.apply_ships = form.ApplyShips;
            research.apply_infrastructure = form.ApplyInfrastructure;

            research.domestic_influence_bonus = form.DomesticInfluence;
            research.foreign_influence_bonus = form.ForeignInfluence;

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
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public  ActionResult Delete(int gameID, int? researchID)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Delete - researchID={0}", researchID));

            var research = Database.Session.Load<DB_research>(researchID);
            if (research == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (research.game_id == null || research.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(research);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}