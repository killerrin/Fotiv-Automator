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
    public class InfrastructureController : DataController
    {
        [HttpGet]
        public  ActionResult Index(int gameID, int? infrastructureID = null)
        { 
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Index - infrastructureID={0}", infrastructureID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = game.GameStatistics.Infrastructure
            });
        }

        [HttpGet]
        public ActionResult Show(int gameID, int? infrastructureID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: View - infrastructureID={0}", infrastructureID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = game.GameStatistics.Infrastructure.Find(x => x.Infrastructure.id == infrastructureID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public ActionResult New(int gameID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: New"));
            var game = GameState.Game;
            return View(new InfrastructureForm
            {
                PossibleUpgrades = GameState.Game.GameStatistics.InfrastructureRaw.Select(x => new Checkbox(x.id, x.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(InfrastructureForm form)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_infrastructure infrastructure = new DB_infrastructure();
            infrastructure.game_id = game.Info.id;
            infrastructure.name = form.Name;
            infrastructure.description = form.Description;
            infrastructure.rp_cost = form.RPCost;

            infrastructure.is_colony = form.IsColony;
            infrastructure.is_military = form.IsMilitary;

            infrastructure.base_health = form.BaseHealth;
            infrastructure.base_regeneration = form.BaseRegeneration;
            infrastructure.base_attack = form.BaseAttack;
            infrastructure.base_special_attack = form.BaseSpecialAttack;
            infrastructure.base_agility = form.BaseAgility;

            infrastructure.influence_bonus = form.InfluenceBonus;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;
            infrastructure.unit_training_bonus = form.UnitTrainingBonus;

            infrastructure.research_slots = form.ResearchSlots;
            infrastructure.ship_construction_slots = form.ShipConstructionSlots;
            infrastructure.colonial_development_slots = form.ColonialDevelopmentSlots;
            infrastructure.unit_training_slots = form.UnitTrainingSlots;

            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Save(infrastructure);

            foreach (var possibleUpgrade in form.PossibleUpgrades)
            {
                if (possibleUpgrade.IsChecked)
                {
                    DB_infrastructure_upgrades upgrade = new DB_infrastructure_upgrades();
                    upgrade.game_id = game.Info.id;
                    upgrade.from_infra_id = infrastructure.id;
                    upgrade.to_infra_id = possibleUpgrade.ID;
                    Database.Session.Save(upgrade);
                }
            }

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public ActionResult Edit(int gameID, int? infrastructureID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Edit - infrastructureID={0}", infrastructureID));
            var game = GameState.Game;

            InfrastructureUpgrade infrastructure = game.GameStatistics.Infrastructure.Find(x => x.Infrastructure.id == infrastructureID);

            var possibleUpgrades = game.GameStatistics.InfrastructureRaw.Select(x => new Checkbox(x.id, x.name, infrastructure.IsUpgrade(x.id))).ToList();
            possibleUpgrades.RemoveAll(x => x.ID == infrastructure.Infrastructure.id);

            return View(new InfrastructureForm
            {
                ID = infrastructure.Infrastructure.id,

                Name        = infrastructure.Infrastructure.name,
                Description = infrastructure.Infrastructure.description,
                RPCost      = infrastructure.Infrastructure.rp_cost,

                IsColony    = infrastructure.Infrastructure.is_colony,
                IsMilitary  = infrastructure.Infrastructure.is_military,

                BaseHealth          = infrastructure.Infrastructure.base_health,
                BaseRegeneration    = infrastructure.Infrastructure.base_regeneration,
                BaseAttack          = infrastructure.Infrastructure.base_attack,
                BaseSpecialAttack   = infrastructure.Infrastructure.base_special_attack,
                BaseAgility         = infrastructure.Infrastructure.base_agility,

                InfluenceBonus = infrastructure.Infrastructure.influence_bonus,

                RPBonus                     = infrastructure.Infrastructure.rp_bonus,
                ScienceBonus                = infrastructure.Infrastructure.science_bonus,
                ShipConstructionBonus       = infrastructure.Infrastructure.ship_construction_bonus,
                ColonialDevelopmentBonus    = infrastructure.Infrastructure.colonial_development_bonus,
                UnitTrainingBonus           = infrastructure.Infrastructure.unit_training_bonus,

                ResearchSlots            = infrastructure.Infrastructure.research_slots,
                ShipConstructionSlots    = infrastructure.Infrastructure.ship_construction_slots,
                ColonialDevelopmentSlots = infrastructure.Infrastructure.colonial_development_slots,
                UnitTrainingSlots        = infrastructure.Infrastructure.unit_training_slots,

                GMNotes = infrastructure.Infrastructure.gmnotes,

                PossibleUpgrades = possibleUpgrades
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(InfrastructureForm form)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Edit - infrastructureID={0}", form.ID));
            var game = GameState.Game;

            DB_infrastructure infrastructure = game.GameStatistics.InfrastructureRaw.Find(x => x.id == form.ID);
            if (infrastructure.game_id == null || infrastructure.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            infrastructure.name = form.Name;
            infrastructure.description = form.Description;
            infrastructure.rp_cost = form.RPCost;

            infrastructure.is_colony = form.IsColony;
            infrastructure.is_military = form.IsMilitary;

            infrastructure.base_health = form.BaseHealth;
            infrastructure.base_regeneration = form.BaseRegeneration;
            infrastructure.base_attack = form.BaseAttack;
            infrastructure.base_special_attack = form.BaseSpecialAttack;
            infrastructure.base_agility = form.BaseAgility;

            infrastructure.influence_bonus = form.InfluenceBonus;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;
            infrastructure.unit_training_bonus = form.UnitTrainingBonus;

            infrastructure.research_slots = form.ResearchSlots;
            infrastructure.ship_construction_slots = form.ShipConstructionSlots;
            infrastructure.colonial_development_slots = form.ColonialDevelopmentSlots;
            infrastructure.unit_training_slots = form.UnitTrainingSlots;

            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Update(infrastructure);

            var infrastructureUpgrades = Database.Session.Query<DB_infrastructure_upgrades>()
                .Where(x => x.from_infra_id == infrastructure.id)
                .ToList();

            var checkedUpgrades = form.PossibleUpgrades
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_infrastructure_upgrades> toRemove = new List<DB_infrastructure_upgrades>();
            List<Checkbox> toAdd = new List<Checkbox>();

            foreach (var infrastructureUpgrade in infrastructureUpgrades)
            {
                bool foundMatch = false;

                // First determine what to remove
                foreach (var checkBox in checkedUpgrades)
                {
                    // Infrastructure Upgrade is already set
                    if (infrastructureUpgrade.to_infra_id == checkBox.ID)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // No longer an Upgrade to this Infrastructure
                if (!foundMatch)
                    toRemove.Add(infrastructureUpgrade);
            }

            // Next determine what is new
            foreach (var checkBox in checkedUpgrades)
            {
                bool foundMatch = false;
                foreach (var infrastructureUpgrade in infrastructureUpgrades)
                {
                    // Infrastructure Upgrade is already set
                    if (checkBox.ID == infrastructureUpgrade.to_infra_id)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // We have a new upgrade
                if (!foundMatch)
                    toAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in toRemove)
                Database.Session.Delete(remove);
            foreach (var add in toAdd)
                Database.Session.Save(new DB_infrastructure_upgrades(infrastructure.id, add.ID, game.Info.id));

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public  ActionResult Delete(int gameID, int? infrastructureID)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Delete - infrastructureID={0}", infrastructureID));

            var infrastructure = Database.Session.Load<DB_infrastructure>(infrastructureID);
            if (infrastructure == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (infrastructure.game_id == null || infrastructure.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}