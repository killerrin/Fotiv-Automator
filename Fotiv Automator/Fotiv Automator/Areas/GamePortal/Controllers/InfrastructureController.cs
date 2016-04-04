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
    public class InfrastructureController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int? infrastructureID = null)
        { 
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Index - infrastructureID={0}", infrastructureID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = game.GameStatistics.Infrastructure
            });
        }

        [HttpGet]
        public override ActionResult View(int? infrastructureID)
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
        [HttpGet]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: New"));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;
            return View(new InfrastructureForm
            {
                PossibleUpgrades = GameState.Game.GameStatistics.InfrastructureRaw.Select(x => new Checkbox(x.id, x.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(InfrastructureForm form)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;
            if (!game.IsPlayerGM(Auth.User.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

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
            infrastructure.influence = form.Influence;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;
            infrastructure.unit_training_bonus = form.UnitTrainingBonus;

            infrastructure.research_slot = form.ResearchSlot;
            infrastructure.ship_construction_slot = form.ShipConstructionSlot;
            infrastructure.colonial_development_slot = form.ColonialDevelopmentSlot;
            infrastructure.unit_training_slot = form.UnitTrainingSlot;

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
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? infrastructureID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Edit - infrastructureID={0}", infrastructureID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

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
                Influence           = infrastructure.Infrastructure.influence,

                RPBonus                     = infrastructure.Infrastructure.rp_bonus,
                ScienceBonus                = infrastructure.Infrastructure.science_bonus,
                ShipConstructionBonus       = infrastructure.Infrastructure.ship_construction_bonus,
                ColonialDevelopmentBonus    = infrastructure.Infrastructure.colonial_development_bonus,
                UnitTrainingBonus           = infrastructure.Infrastructure.unit_training_bonus,

                ResearchSlot            = infrastructure.Infrastructure.research_slot,
                ShipConstructionSlot    = infrastructure.Infrastructure.ship_construction_slot,
                ColonialDevelopmentSlot = infrastructure.Infrastructure.colonial_development_slot,
                UnitTrainingSlot        = infrastructure.Infrastructure.unit_training_slot,

                GMNotes = infrastructure.Infrastructure.gmnotes,

                PossibleUpgrades = possibleUpgrades
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureForm form, int? infrastructureID)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Edit - infrastructureID={0}", infrastructureID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            DB_infrastructure infrastructure = game.GameStatistics.InfrastructureRaw.Find(x => x.id == infrastructureID);
            if ((infrastructure.game_id == null || infrastructure.game_id != game.Info.id) && !game.IsPlayerGM(Auth.User.id) && !User.IsInRole("Admin"))
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
            infrastructure.influence = form.Influence;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;
            infrastructure.unit_training_bonus = form.UnitTrainingBonus;

            infrastructure.research_slot = form.ResearchSlot;
            infrastructure.ship_construction_slot = form.ShipConstructionSlot;
            infrastructure.colonial_development_slot = form.ColonialDevelopmentSlot;
            infrastructure.unit_training_slot = form.UnitTrainingSlot;

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
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? infrastructureID)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Delete - infrastructureID={0}", infrastructureID));

            var infrastructure = Database.Session.Load<DB_infrastructure>(infrastructureID);
            if (infrastructure == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if ((infrastructure.game_id == null || infrastructure.game_id != game.Info.id) && !game.IsPlayerGM(Auth.User.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}