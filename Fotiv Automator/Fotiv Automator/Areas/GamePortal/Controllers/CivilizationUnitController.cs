using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Infrastructure.CustomControllers;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class CivilizationUnitController : DataController
    {
        [HttpGet]
        public  ActionResult Show(int gameID, int civilizationID, int? civilizationUnitID)
        {
            Debug.WriteLine($"GET: Civilization Units Controller: View - {nameof(civilizationUnitID)}={civilizationUnitID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var unit = FindCivilizationUnit(civilizationUnitID);

            return View(new ViewCivilizationUnits
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Unit = unit,
                PlayerOwnsCivilization = game.GetCivilization(unit.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID, int civilizationID)
        {
            Debug.WriteLine($"GET: Civilization Units Controller: New");

            Game game = GameState.Game;
            var civilization = game.GetCivilization(civilizationID);

            List<Checkbox> species = new List<Checkbox>();
            species.Add(new Checkbox(-1, "None", true));
            species.AddRange(civilization.SpeciesInfo.Select(x => new Checkbox(x.id, x.name, false)).ToList());

            return View(new CivilizationUnitsForm
            {
                CivilizationID = civilizationID,
                Units = GameState.Game.GameStatistics.Units.Select(x => new Checkbox(x.Info.id, x.Info.name, false)).ToList(),
                Species = species
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(CivilizationUnitsForm form)
        {
            Debug.WriteLine($"POST: Civilization Units Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var dbUnit = game.GameStatistics.Units
                .Where(x => x.Info.id == form.SelectedUnitID.Value)
                .FirstOrDefault();
            if (dbUnit == null)
                return View(form);

            DB_civilization_units unit = new DB_civilization_units();
            unit.game_id = game.ID;
            unit.unit_id = dbUnit.Info.id;
            unit.species_id = (form.SelectedSpeciesID == -1) ? null : form.SelectedSpeciesID;
            unit.civilization_id = form.CivilizationID.Value;
            unit.name = form.Name;
            unit.current_health = dbUnit.Info.base_health;
            unit.experience = form.Experience;
            unit.gmnotes = form.GMNotes;
            Database.Session.Save(unit);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int civilizationID, int? civilizationUnitID)
        {
            Debug.WriteLine($"GET: Civilization Units Controller: Edit - {nameof(civilizationUnitID)}={civilizationUnitID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var unit = FindCivilizationUnit(civilizationUnitID);
            var unitCheckBoxes = GameState.Game.GameStatistics.UnitsRaw
                .Select(x => new Checkbox(x.id, x.name, x.id == unit.CivilizationInfo.unit_id))
                .ToList();

            var civilization = game.GetCivilization(unit.CivilizationInfo.civilization_id);
            List<Checkbox> speciesCheckboxes = new List<Checkbox>();
            speciesCheckboxes.Add(new Checkbox(-1, "None", false));
            speciesCheckboxes.AddRange(civilization.SpeciesInfo.Select(x => new Checkbox(x.id, x.name, x.id == unit.CivilizationInfo.species_id)).ToList());

            if (speciesCheckboxes.Where(x => x.IsChecked).FirstOrDefault() == null)
                speciesCheckboxes[0].IsChecked = true;

            return View(new CivilizationUnitsForm
            {
                ID = unit.CivilizationInfo.id,
                CivilizationID = unit.CivilizationInfo.civilization_id,
                Name = unit.CivilizationInfo.name,
                CurrentHealth = unit.CivilizationInfo.current_health,
                Experience = unit.CivilizationInfo.experience,
                GMNotes = unit.CivilizationInfo.gmnotes,

                Units = unitCheckBoxes,
                SelectedUnitID = unitCheckBoxes.Where(x => x.IsChecked).First().ID,

                Species = speciesCheckboxes,
                SelectedSpeciesID = speciesCheckboxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(CivilizationUnitsForm form)
        {
            Debug.WriteLine($"POST: Civilization Units Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_units unit = FindCivilizationUnit(form.ID).CivilizationInfo;
            if (unit.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var dbUnit = game.GameStatistics.UnitsRaw
                .Where(x => x.id == form.SelectedUnitID.Value)
                .FirstOrDefault();
            if (dbUnit == null)
                return View(form);

            unit.name = form.Name;
            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                unit.civilization_id = form.CivilizationID.Value;
                unit.unit_id = dbUnit.id;
                unit.species_id = (form.SelectedSpeciesID == -1) ? null : form.SelectedSpeciesID;
                unit.current_health = form.CurrentHealth;
                unit.experience = form.Experience;
                unit.gmnotes = form.GMNotes;
            }
            Database.Session.Update(unit);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public  ActionResult Delete(int gameID, int civilizationID, int? civilizationUnitID)
        {
            Debug.WriteLine($"POST: Civilization Units Controller: Delete - {nameof(civilizationUnitID)}={civilizationUnitID}");

            var ship = Database.Session.Load<DB_civilization_units>(civilizationUnitID);
            if (ship == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(ship.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                ship.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(ship);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private CivilizationUnit FindCivilizationUnit(int? civilizationUnitID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.CompletedUnitsRaw)
                    if (rnd?.CivilizationInfo.id == civilizationUnitID)
                        return rnd;

            return null;
        }
        #endregion
    }
}