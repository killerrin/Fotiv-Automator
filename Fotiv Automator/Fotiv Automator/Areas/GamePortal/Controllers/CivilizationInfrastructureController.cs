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
    public class CivilizationInfrastructureController : DataController
    {
        [HttpGet]
        public  ActionResult Show(int gameID, int civilizationID, int? civilizationInfrastructureID)
        {
            Debug.WriteLine($"GET: Civilization Infrastructure Controller: View - {nameof(civilizationInfrastructureID)}={civilizationInfrastructureID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var infrastructure = FindCivilizationInfrastructure(civilizationInfrastructureID);

            return View(new ViewCivilizationInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = infrastructure,
                PlayerOwnsCivilization = game.GetCivilization(infrastructure.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public  ActionResult New(int gameID, int civilizationID)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: New");
            return View(new CivilizationInfrastructureForm
            {
                CivilizationID = civilizationID,
                Infrastructure = GameState.Game.GameStatistics.Infrastructure.Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(CivilizationInfrastructureForm form)
        {
            Debug.WriteLine($"POST: Civilization Infrastructure Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var planet = game.Sector.PlanetFromID(form.PlanetID.Value);
            var dbStruct = game.GameStatistics.InfrastructureRaw
                .Where(x => x.id == form.SelectedInfrastructureID.Value)
                .FirstOrDefault();

            if (planet == null || dbStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            DB_civilization_infrastructure infrastructure = new DB_civilization_infrastructure();
            infrastructure.game_id = game.ID;
            infrastructure.civilization_id = form.CivilizationID.Value;
            infrastructure.planet_id = planet.PlanetID;
            infrastructure.struct_id = dbStruct.id;
            infrastructure.name = form.Name;
            infrastructure.current_health = dbStruct.base_health;
            infrastructure.experience = 0;
            infrastructure.can_upgrade = form.CanUpgrade;
            infrastructure.is_military = form.IsMilitary;
            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Save(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int civilizationID, int? civilizationInfrastructureID)
        {
            Debug.WriteLine($"GET: Civilization Infrastructure Controller: Edit - {nameof(civilizationInfrastructureID)}={civilizationInfrastructureID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var infrastructure = FindCivilizationInfrastructure(civilizationInfrastructureID);
            var infrastructureCheckBoxes = GameState.Game.GameStatistics.Infrastructure
                .Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, x.Infrastructure.id == infrastructure.CivilizationInfo.struct_id)).ToList();

            return View(new CivilizationInfrastructureForm
            {
                ID = infrastructure.CivilizationInfo.id,
                CivilizationID = infrastructure.CivilizationInfo.civilization_id,
                PlanetID = infrastructure.CivilizationInfo.planet_id,

                Name = infrastructure.CivilizationInfo.name,
                CurrentHealth = infrastructure.CivilizationInfo.current_health,
                Experience = infrastructure.CivilizationInfo.experience,
                CanUpgrade = infrastructure.CivilizationInfo.can_upgrade,
                IsMilitary = infrastructure.CivilizationInfo.is_military,
                GMNotes = infrastructure.CivilizationInfo.gmnotes,

                Infrastructure = infrastructureCheckBoxes,
                SelectedInfrastructureID = infrastructureCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(CivilizationInfrastructureForm form)
        {
            Debug.WriteLine($"POST: Civilization Infrastructure Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_infrastructure infrastructure = FindCivilizationInfrastructure(form.ID).CivilizationInfo;
            if (infrastructure.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var planet = game.Sector.PlanetFromID(form.PlanetID.Value);
            var dbStruct = game.GameStatistics.InfrastructureRaw
                .Where(x => x.id == form.SelectedInfrastructureID.Value)
                .FirstOrDefault();

            if (planet == null || dbStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            infrastructure.name = form.Name;
            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                infrastructure.civilization_id = form.CivilizationID.Value;
                infrastructure.planet_id = planet.PlanetID;
                infrastructure.struct_id = dbStruct.id;
                infrastructure.current_health = form.CurrentHealth;
                infrastructure.experience = form.Experience;
                infrastructure.can_upgrade = form.CanUpgrade;
                infrastructure.is_military = form.IsMilitary;
                infrastructure.gmnotes = form.GMNotes;
            }
            Database.Session.Update(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public  ActionResult Delete(int gameID, int civilizationID, int? civilizationInfrastructureID)
        {
            Debug.WriteLine($"POST: Civilization Infrastructure Controller: Delete - {nameof(civilizationInfrastructureID)}={civilizationInfrastructureID}");

            var infrastructure = Database.Session.Load<DB_civilization_infrastructure>(civilizationInfrastructureID);
            if (infrastructure == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(infrastructure.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                infrastructure.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private Models.Game.CivilizationInfrastructure FindCivilizationInfrastructure(int? civilizationInfrastructureID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.CompletedInfrastructure)
                    if (rnd?.CivilizationInfo.id == civilizationInfrastructureID)
                        return rnd;

            return null;
        }
        #endregion
    }
}