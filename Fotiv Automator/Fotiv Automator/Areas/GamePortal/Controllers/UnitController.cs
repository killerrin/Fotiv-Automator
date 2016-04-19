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
    public class UnitController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? unitID = null)
        { 
            Debug.WriteLine(string.Format("GET: Unit Controller: Index - unitID={0}", unitID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexUnits
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Units = game.GameStatistics.Units
            });
        }

        [HttpGet]
        public override ActionResult Show(int? unitID)
        {
            Debug.WriteLine(string.Format("GET: Unit Controller: View - unitID={0}", unitID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewUnit
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Unit = game.GameStatistics.Units.Find(x => x.Info.id == unitID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Unit Controller: New"));

            var game = GameState.Game;
            var categories = new List<Checkbox>();
            categories.Add(new Checkbox(-1, "None", true));
            foreach (var category in game.GameStatistics.UnitCategoriesRaw)
                categories.Add(new Checkbox(category.id, category.name, false));

            return View(new UnitForm
            {
                Categories = categories,
                UnitTypes = GetUnitTypesList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(UnitForm form)
        {
            Debug.WriteLine(string.Format("POST: Unit Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_units unit = new DB_units();
            unit.game_id = game.Info.id;
            unit.unit_category_id = (form.SelectedCategoryID == -1) ? null : form.SelectedCategoryID;

            unit.name = form.Name;
            unit.unit_type = GetUnitTypesList().Where(x => x.ID == form.SelectedUnitTypeID).First().Name;
            unit.description = form.Description;
            unit.rp_cost = form.RPCost;
            unit.number_to_build = form.NumberToBuild;

            unit.is_space_unit = form.IsSpaceUnit;
            unit.can_embark = form.CanEmbark;

            unit.can_attack_ground_units = form.CanAttackGroundUnits;
            unit.can_attack_boats = form.CanAttackBoats;
            unit.can_attack_planes = form.CanAttackPlanes;
            unit.can_attack_spaceships = form.CanAttackSpaceships;

            unit.embarking_slots = form.EmbarkingSlots;
            unit.negate_damage = form.NegateDamage;

            unit.base_health = form.BaseHealth;
            unit.base_regeneration = form.BaseRegeneration;
            unit.base_attack = form.BaseAttack;
            unit.base_special_attack = form.BaseSpecialAttack;
            unit.base_agility = form.BaseAgility;

            unit.gmnotes = form.GMNotes;
            Database.Session.Save(unit);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? unitID)
        {
            Debug.WriteLine(string.Format("GET: Ship Controller: Edit - unitID={0}", unitID));
            var game = GameState.Game;

            var unit = game.GameStatistics.UnitsRaw.Find(x => x.id == unitID);

            var categories = new List<Checkbox>();
            categories.Add(new Checkbox(-1, "None", false));
            foreach (var category in game.GameStatistics.UnitCategoriesRaw)
                categories.Add(new Checkbox(category.id, category.name, category.id == unit.unit_category_id));

            var selected = categories.Where(x => x.IsChecked).ToList();
            if (selected.Count == 0) categories[0].IsChecked = true;

            var unitTypes = GetUnitTypesList();
            foreach (var checkbox in unitTypes)
                if (checkbox.Name == unit.unit_type)
                    checkbox.IsChecked = true;

            return View(new UnitForm
            {
                ID = unit.id,

                Name = unit.name,
                Description = unit.description,
                RPCost = unit.rp_cost,
                NumberToBuild = unit.number_to_build,

                IsSpaceUnit = unit.is_space_unit,
                CanEmbark = unit.can_embark,

                CanAttackGroundUnits = unit.can_attack_ground_units,
                CanAttackBoats = unit.can_attack_boats,
                CanAttackPlanes = unit.can_attack_planes,
                CanAttackSpaceships = unit.can_attack_spaceships,

                EmbarkingSlots = unit.embarking_slots,
                NegateDamage = unit.negate_damage,

                BaseHealth = unit.base_health,
                BaseRegeneration = unit.base_regeneration,
                BaseAttack = unit.base_attack,
                BaseSpecialAttack = unit.base_special_attack,
                BaseAgility = unit.base_agility,

                GMNotes = unit.gmnotes,

                Categories = categories,
                UnitTypes = unitTypes,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(UnitForm form, int? unitID)
        {
            Debug.WriteLine(string.Format("POST: Ship Controller: Edit - unitID={0}", unitID));
            var game = GameState.Game;

            var unit = game.GameStatistics.UnitsRaw.Find(x => x.id == unitID);
            if (unit.game_id == null || unit.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            unit.unit_category_id = (form.SelectedCategoryID == -1) ? null : form.SelectedCategoryID;

            unit.name = form.Name;
            unit.unit_type = GetUnitTypesList().Where(x => x.ID == form.SelectedUnitTypeID).First().Name;
            unit.description = form.Description;
            unit.rp_cost = form.RPCost;
            unit.number_to_build = form.NumberToBuild;

            unit.is_space_unit = form.IsSpaceUnit;
            unit.can_embark = form.CanEmbark;

            unit.can_attack_ground_units = form.CanAttackGroundUnits;
            unit.can_attack_boats = form.CanAttackBoats;
            unit.can_attack_planes = form.CanAttackPlanes;
            unit.can_attack_spaceships = form.CanAttackSpaceships;

            unit.embarking_slots = form.EmbarkingSlots;
            unit.negate_damage = form.NegateDamage;

            unit.base_health = form.BaseHealth;
            unit.base_regeneration = form.BaseRegeneration;
            unit.base_attack = form.BaseAttack;
            unit.base_special_attack = form.BaseSpecialAttack;
            unit.base_agility = form.BaseAgility;

            unit.gmnotes = form.GMNotes;
            Database.Session.Update(unit);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? unitID)
        {
            Debug.WriteLine(string.Format("POST: Ship Controller: Delete - unitID={0}", unitID));

            var ship = Database.Session.Load<DB_units>(unitID);
            if (ship == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (ship.game_id == null || ship.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(ship);
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }

        private List<Checkbox> GetUnitTypesList()
        {
            var unitTypes = new List<Checkbox>();
            unitTypes.Add(new Checkbox(1, "Ground Unit", false));
            unitTypes.Add(new Checkbox(2, "Boat", false));
            unitTypes.Add(new Checkbox(3, "Plane", false));
            unitTypes.Add(new Checkbox(4, "Spaceship", false));
            return unitTypes;
        }
    }
}