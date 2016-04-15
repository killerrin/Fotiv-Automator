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
    public class RnDColonialDevelopmentController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: Index - {nameof(civilizationID)}={civilizationID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (civilizationID == null) return RedirectToRoute("home");

            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new IndexRnDColonialDevelopment
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = civilization.Assets.IncompleteInfrastructure,
                CivilizationID = civilization.Info.id,
                CivilizationName = civilization.Info.name,

                PlayerOwnsCivilization = civilization.PlayerOwnsCivilization(user.id)
            });
        }

        [HttpGet]
        public override ActionResult Show(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: View - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var infrastructure = FindRNDInfrastructure(rndColonialDevelopmentID);

            return View(new ViewRnDColonialDevelopment
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = infrastructure,
                PlayerOwnsCivilization = game.GetCivilization(infrastructure.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: New");
            return View(new RnDColonialDevelopmentForm
            {
                CivilizationID = civilizationID,
                Infrastructure = GameState.Game.GameStatistics.Infrastructure.Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDColonialDevelopmentForm form)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;
            if (!game.GetCivilization(form.CivilizationID.Value).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

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
            infrastructure.build_percentage = form.BuildPercentage;
            infrastructure.current_health = dbStruct.base_health;
            infrastructure.can_upgrade = form.CanUpgrade;
            infrastructure.is_military = form.IsMilitary;
            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Save(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: Edit - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var infrastructure = FindRNDInfrastructure(rndColonialDevelopmentID);
            var infrastructureCheckBoxes = GameState.Game.GameStatistics.Infrastructure
                .Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, x.Infrastructure.id == infrastructure.CivilizationInfo.struct_id)).ToList();

            return View(new RnDColonialDevelopmentForm
            {
                ID = infrastructure.CivilizationInfo.id,
                CivilizationID = infrastructure.CivilizationInfo.civilization_id,
                PlanetID = infrastructure.CivilizationInfo.planet_id,

                Name = infrastructure.CivilizationInfo.name,
                BuildPercentage = infrastructure.CivilizationInfo.build_percentage,
                CurrentHealth = infrastructure.CivilizationInfo.current_health,
                CanUpgrade = infrastructure.CivilizationInfo.can_upgrade,
                IsMilitary = infrastructure.CivilizationInfo.is_military,
                GMNotes = infrastructure.CivilizationInfo.gmnotes,

                Infrastructure = infrastructureCheckBoxes,
                SelectedInfrastructureID = infrastructureCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RnDColonialDevelopmentForm form, int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_infrastructure infrastructure = FindRNDInfrastructure(rndColonialDevelopmentID).CivilizationInfo;
            if (!RequireGMAdminAttribute.IsGMOrAdmin() || infrastructure.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var planet = game.Sector.PlanetFromID(form.PlanetID.Value);
            var dbStruct = game.GameStatistics.InfrastructureRaw
                .Where(x => x.id == form.SelectedInfrastructureID.Value)
                .FirstOrDefault();

            if (planet == null || dbStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            infrastructure.civilization_id = form.CivilizationID.Value;
            infrastructure.planet_id = planet.PlanetID;
            infrastructure.struct_id = dbStruct.id;
            infrastructure.name = form.Name;
            infrastructure.build_percentage = form.BuildPercentage;
            infrastructure.current_health = form.CurrentHealth;
            infrastructure.can_upgrade = form.CanUpgrade;
            infrastructure.is_military = form.IsMilitary;
            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Update(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: Delete - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");

            var infrastructure = Database.Session.Load<DB_civilization_infrastructure>(rndColonialDevelopmentID);
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
        private Models.Game.Infrastructure FindRNDInfrastructure(int? rndColonialDevelopmentID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.InfrastructureRaw)
                    if (rnd?.CivilizationInfo.id == rndColonialDevelopmentID)
                        return rnd;

            return null;
        }
        #endregion
    }
}