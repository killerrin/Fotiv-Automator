﻿using Fotiv_Automator.Areas.GamePortal.Models.Game;
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
    public class CivilizationRnDUnitController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? rndUnitID)
        {
            Debug.WriteLine($"GET: Civilization RnD Unit Controller: View - {nameof(rndUnitID)}={rndUnitID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var unit = FindRNDCivilizationUnit(rndUnitID);

            return View(new ViewCivilizationRnDUnit
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Unit = unit,
                PlayerOwnsCivilization = game.GetCivilization(unit.Info.civilization_id).PlayerOwnsCivilization(user.id),
            });
        }

        #region New
        [HttpGet]
        public ActionResult NewUnit(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Unit Controller: New");
            Game game = GameState.Game;
            var civilization = game.GetCivilization(civilizationID.Value);

            var species = civilization.SpeciesInfo.Select(x => new Checkbox(x.id, x.name, false)).ToList();
            species.Insert(0, new Checkbox(-1, "None", true));

            return View(new RnDUnitForm
            {
                CivilizationID = civilizationID,
                Units = game.GameStatistics.UnitsRaw
                    .Where(x => x.is_space_unit == false)
                    .Select(x => new Checkbox(x.id, x.name, false))
                    .ToList(),

                BuildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.unit_training_slot)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", false))
                    .ToList(),
                Species = species
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewUnit(RnDUnitForm form)
        {
            Debug.WriteLine($"POST: Civilization RnD Unit Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var dbUnit = game.GameStatistics.UnitsRaw
                .Where(x => x.id == form.SelectedUnitID.Value)
                .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (dbUnit == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            // Check to ensure there are slots remaining
            var civilization = game.GetCivilization(form.CivilizationID.Value);
            if (dbUnit.is_space_unit)
            {
                if (!civilization.Assets.HasShipConstructionSlots)
                    return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
            }
            else
            {
                if (!civilization.Assets.HasUnitTrainingSlots)
                    return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
            }

            // Save the Unit to the DB
            DB_civilization_rnd_units unit = new DB_civilization_rnd_units();
            unit.game_id = game.ID;
            unit.civilization_id = form.CivilizationID.Value;
            unit.unit_id = dbUnit.id;
            unit.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
            unit.species_id = form.SelectedSpeciesID == -1 ? null : form.SelectedSpeciesID;
            unit.name = form.Name;
            unit.build_percentage = form.BuildPercentage;
            Database.Session.Save(unit);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }

        [HttpGet]
        public ActionResult NewShip(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Unit Controller: New");
            Game game = GameState.Game;
            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new RnDUnitForm
            {
                CivilizationID = civilizationID,
                Units = game.GameStatistics.UnitsRaw.Where(x => x.is_space_unit == true).Select(x => new Checkbox(x.id, x.name, false)).ToList(),
                BuildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.ship_construction_slot)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", false))
                    .ToList(),
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewShip(RnDUnitForm form)
        {
            return NewUnit(form);
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public ActionResult EditUnit(int? rndUnitID)
        {
            Debug.WriteLine($"GET: Civilization RND Unit Controller: Edit - {nameof(rndUnitID)}={rndUnitID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var unit = FindRNDCivilizationUnit(rndUnitID);
            var unitCheckBoxes = game.GameStatistics.UnitsRaw
                .Where(x => x.is_space_unit == false)
                .Select(x => new Checkbox(x.id, x.name, x.id == unit.Info.unit_id))
                .ToList();

            var civilization = game.GetCivilization(unit.Info.civilization_id);
            var buildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.unit_training_slot == true)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", x.CivilizationInfo.id == unit.Info.civ_struct_id))
                    .ToList();

            var species = civilization.SpeciesInfo.Select(x => new Checkbox(x.id, x.name, x.id == unit.Info.species_id)).ToList();
            species.Insert(0, new Checkbox(-1, "None", species.Where(x => x.IsChecked).FirstOrDefault() == null));

            return View(new RnDUnitForm
            {
                ID = unit.Info.id,
                CivilizationID = unit.Info.civilization_id,

                Name = unit.Info.name,
                BuildPercentage = unit.Info.build_percentage,

                Units = unitCheckBoxes,
                SelectedUnitID = unitCheckBoxes.Where(x => x.IsChecked).First().ID,

                BuildAtInfrastructure = buildAtInfrastructure,
                SelectedBuildAtInfrastructureID = buildAtInfrastructure.Where(x => x.IsChecked).First().ID,

                Species = species,
                SelectedSpeciesID = species.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult EditUnit(RnDUnitForm form, int? rndUnitID)
        {
            Debug.WriteLine($"POST: Civilization RND Unit Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_rnd_units unit = FindRNDCivilizationUnit(rndUnitID).Info;
            if (unit.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var dbUnit = game.GameStatistics.UnitsRaw
               .Where(x => x.id == form.SelectedUnitID.Value)
               .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (dbUnit == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            unit.name = form.Name;
            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                unit.civilization_id = form.CivilizationID.Value;
                unit.unit_id = dbUnit.id;
                unit.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
                unit.species_id = form.SelectedSpeciesID == -1 ? null : form.SelectedSpeciesID;
                unit.build_percentage = form.BuildPercentage;
            }
            Database.Session.Update(unit);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }

        [HttpGet, RequireGMAdmin]
        public ActionResult EditShip(int? rndUnitID)
        {
            Debug.WriteLine($"GET: Civilization RND Unit Controller: Edit - {nameof(rndUnitID)}={rndUnitID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var unit = FindRNDCivilizationUnit(rndUnitID);
            var unitCheckBoxes = game.GameStatistics.UnitsRaw
                .Where(x => x.is_space_unit == true)
                .Select(x => new Checkbox(x.id, x.name, x.id == unit.Info.unit_id))
                .ToList();

            var civilization = game.GetCivilization(unit.Info.civilization_id);
            var buildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.ship_construction_slot == true)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", x.CivilizationInfo.id == unit.Info.civ_struct_id))
                    .ToList();

            return View(new RnDUnitForm
            {
                ID = unit.Info.id,
                CivilizationID = unit.Info.civilization_id,

                Name = unit.Info.name,
                BuildPercentage = unit.Info.build_percentage,

                Units = unitCheckBoxes,
                SelectedUnitID = unitCheckBoxes.Where(x => x.IsChecked).First().ID,

                BuildAtInfrastructure = buildAtInfrastructure,
                SelectedBuildAtInfrastructureID = buildAtInfrastructure.Where(x => x.IsChecked).First().ID,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult EditShip(RnDUnitForm form, int? rndUnitID)
        {
            return EditUnit(form, rndUnitID);
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndUnitID)
        {
            Debug.WriteLine($"POST: Civilization RND Unit Controller: Delete - {nameof(rndUnitID)}={rndUnitID}");

            var unit = Database.Session.Load<DB_civilization_rnd_units>(rndUnitID);
            if (unit == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(unit.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                unit.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(unit);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private RnDUnit FindRNDCivilizationUnit(int? rndUnitID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.IncompleteUnitsRaw)
                    if (rnd?.Info.id == rndUnitID)
                        return rnd;

            return null;
        }
        #endregion
    }
}