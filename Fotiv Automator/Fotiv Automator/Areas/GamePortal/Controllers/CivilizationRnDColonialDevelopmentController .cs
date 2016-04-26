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
    public class CivilizationRnDColonialDevelopmentController : DataController
    {
        [HttpGet]
        public  ActionResult Show(int gameID, int civilizationID, int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: Civilization RnD Colonial Development Controller: View - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var infrastructure = FindRNDCivilizationInfrastructure(rndColonialDevelopmentID);

            return View(new ViewCivilizationRnDColonialDevelopment
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = infrastructure,
                PlayerOwnsCivilization = game.GetCivilization(infrastructure.Info.civilization_id).PlayerOwnsCivilization(user.id),
            });
        }

        #region New
        [HttpGet]
        public  ActionResult New(int gameID, int civilizationID)
        {
            Debug.WriteLine($"GET: Civilization R&D Colonial Development Controller: New");
            Game game = GameState.Game;
            var civilization = game.GetCivilization(civilizationID);

            return View(new RnDColonialDevelopmentForm
            {
                CivilizationID = civilizationID,
                Infrastructure = game.GameStatistics.Infrastructure
                    .Where(x => civilization.CanAfford(x.Infrastructure.rp_cost))
                    .Select(x => new Checkbox(x.Infrastructure.id, $"{x.Infrastructure.rp_cost}RP - {x.Infrastructure.name}", false))
                    .ToList(),
                BuildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.colonial_development_slots > 0)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", false))
                    .ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDColonialDevelopmentForm form)
        {
            Debug.WriteLine($"POST: Civilization R&D Colonial Development Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var planet = game.Sector.PlanetFromID(form.PlanetID.Value);
            var dbStruct = game.GameStatistics.InfrastructureRaw
                .Where(x => x.id == form.SelectedInfrastructureID.Value)
                .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (planet == null || dbStruct == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            DB_civilization_rnd_infrastructure infrastructure = new DB_civilization_rnd_infrastructure();
            infrastructure.game_id = game.ID;
            infrastructure.civilization_id = form.CivilizationID.Value;
            infrastructure.planet_id = planet.PlanetID;
            infrastructure.struct_id = dbStruct.id;
            infrastructure.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
            infrastructure.name = form.Name;
            infrastructure.build_percentage = form.BuildPercentage;
            Database.Session.Save(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int civilizationID, int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: Civilization R&D Colonial Development Controller: Edit - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var infrastructure = FindRNDCivilizationInfrastructure(rndColonialDevelopmentID);
            var infrastructureCheckBoxes = GameState.Game.GameStatistics.Infrastructure
                .Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, x.Infrastructure.id == infrastructure.Info.struct_id)).ToList();

            var civilization = game.GetCivilization(infrastructure.Info.civilization_id);
            var buildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.colonial_development_slots > 0)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", x.CivilizationInfo.id == infrastructure.Info.civ_struct_id))
                    .ToList();

            return View(new RnDColonialDevelopmentForm
            {
                ID = infrastructure.Info.id,
                CivilizationID = infrastructure.Info.civilization_id,
                PlanetID = infrastructure.Info.planet_id,

                Name = infrastructure.Info.name,
                BuildPercentage = infrastructure.Info.build_percentage,

                Infrastructure = infrastructureCheckBoxes,
                SelectedInfrastructureID = infrastructureCheckBoxes.Where(x => x.IsChecked).First().ID,

                BuildAtInfrastructure = buildAtInfrastructure,
                SelectedBuildAtInfrastructureID = buildAtInfrastructure.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(RnDColonialDevelopmentForm form)
        {
            Debug.WriteLine($"POST: Civilization R&D Colonial Development Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_rnd_infrastructure infrastructure = FindRNDCivilizationInfrastructure(form.ID).Info;
            if (infrastructure.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var planet = game.Sector.PlanetFromID(form.PlanetID.Value);
            var dbStruct = game.GameStatistics.InfrastructureRaw
                .Where(x => x.id == form.SelectedInfrastructureID.Value)
                .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (planet == null || dbStruct == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            infrastructure.name = form.Name;
            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                infrastructure.civilization_id = form.CivilizationID.Value;
                infrastructure.planet_id = planet.PlanetID;
                infrastructure.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
                infrastructure.struct_id = dbStruct.id;
                infrastructure.build_percentage = form.BuildPercentage;
            }
            Database.Session.Update(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public  ActionResult Delete(int gameID, int civilizationID, int? civilizationInfrastructureID)
        {
            Debug.WriteLine($"POST: Civilization R&D Colonial Development Controller: Delete - {nameof(civilizationInfrastructureID)}={civilizationInfrastructureID}");

            var infrastructure = Database.Session.Load<DB_civilization_rnd_infrastructure>(civilizationInfrastructureID);
            if (infrastructure == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var civilization = game.GetCivilization(infrastructure.civilization_id);

            if ((!civilization.PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                infrastructure.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            // Calculate the Refund
            var rndInfrastructure = FindRNDCivilizationInfrastructure(infrastructure.id);
            int refund = civilization.CalculateRefund(rndInfrastructure.BeingBuilt.Infrastructure.rp_cost, infrastructure.build_percentage);

            // Now Delete the Research
            Database.Session.Delete(infrastructure);

            // Then Update the Refund
            civilization.Info.rp += refund;
            Database.Session.Update(civilization.Info);

            // Flush for Good Measure
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private RnDInfrastructure FindRNDCivilizationInfrastructure(int? rndColonialDevelopmentID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.IncompleteInfrastructure)
                    if (rnd?.Info.id == rndColonialDevelopmentID)
                        return rnd;

            return null;
        }
        #endregion
    }
}