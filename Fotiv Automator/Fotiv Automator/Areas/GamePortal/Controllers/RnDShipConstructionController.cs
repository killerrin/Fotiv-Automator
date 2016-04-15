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
    public class RnDShipConstructionController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Research Controller: Index - civilizationID={civilizationID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (civilizationID == null) return RedirectToRoute("home");

            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new IndexRnDShipConstruction
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Ships = civilization.Assets.IncompleteShips,
                CivilizationID = civilization.Info.id,
                CivilizationName = civilization.Info.name,

                PlayerOwnsCivilization = civilization.PlayerOwnsCivilization(user.id)
            });
        }

        [HttpGet]
        public override ActionResult Show(int? rndShipConstructionID)
        {
            Debug.WriteLine($"GET: R&D Research Controller: View - {nameof(rndShipConstructionID)}={rndShipConstructionID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var ship = FindRNDShip(rndShipConstructionID);

            return View(new ViewRnDShipConstruction
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Ship = ship,
                PlayerOwnsCivilization = game.GetCivilization(ship.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Research Controller: New");
            return View(new RnDShipConstructionForm
            {
                CivilizationID = civilizationID,
                Ships = GameState.Game.GameStatistics.Ships.Select(x => new Checkbox(x.Info.id, x.Info.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDShipConstructionForm form)
        {
            Debug.WriteLine($"POST: R&D Research Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;
            if (!game.GetCivilization(form.CivilizationID.Value).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var dbShip = game.GameStatistics.ShipsRaw
                .Where(x => x.id == form.SelectedShipID.Value)
                .FirstOrDefault();
            if (dbShip == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            DB_civilization_ships ship = new DB_civilization_ships();
            ship.game_id = game.ID;
            ship.ship_id = dbShip.id;
            ship.civilization_id = form.CivilizationID.Value;
            ship.name = form.Name;
            ship.build_percentage = form.BuildPercentage;
            ship.current_health = dbShip.base_health;
            ship.command_and_control = form.CommandAndControl;
            ship.gmnotes = form.GMNotes;
            Database.Session.Save(ship);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? rndShipConstructionID)
        {
            Debug.WriteLine($"GET: R&D Research Controller: Edit - {nameof(rndShipConstructionID)}={rndShipConstructionID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var ship = FindRNDShip(rndShipConstructionID);
            var shipsCheckBoxes = GameState.Game.GameStatistics.Ships
                .Select(x => new Checkbox(x.Info.id, x.Info.name, x.Info.id == ship.CivilizationInfo.ship_id))
                .ToList();

            return View(new RnDShipConstructionForm
            {
                ID = ship.CivilizationInfo.id,
                CivilizationID = ship.CivilizationInfo.civilization_id,
                Name = ship.CivilizationInfo.name,
                BuildPercentage = ship.CivilizationInfo.build_percentage,
                CurrentHealth = ship.CivilizationInfo.current_health,
                CommandAndControl = ship.CivilizationInfo.command_and_control,
                GMNotes = ship.CivilizationInfo.gmnotes,

                Ships = shipsCheckBoxes,
                SelectedShipID = shipsCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RnDShipConstructionForm form, int? rndShipConstructionID)
        {
            Debug.WriteLine($"POST: R&D Research Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_ships ship = FindRNDShip(rndShipConstructionID).CivilizationInfo;
            if (!RequireGMAdminAttribute.IsGMOrAdmin() || ship.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var dbShip = game.GameStatistics.ShipsRaw
                .Where(x => x.id == form.SelectedShipID.Value)
                .FirstOrDefault();
            if (dbShip == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            ship.ship_id = dbShip.id;
            ship.civilization_id = form.CivilizationID.Value;
            ship.name = form.Name;
            ship.build_percentage = form.BuildPercentage;
            ship.current_health = dbShip.base_health;
            ship.command_and_control = form.CommandAndControl;
            ship.gmnotes = form.GMNotes;
            Database.Session.Update(ship);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndShipConstructionID)
        {
            Debug.WriteLine($"POST: R&D Research Controller: Delete - {nameof(rndShipConstructionID)}={rndShipConstructionID}");

            var ship = Database.Session.Load<DB_civilization_ships>(rndShipConstructionID);
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
        private CivilizationShip FindRNDShip(int? rndShipConstructionID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.ShipsRaw)
                    if (rnd?.CivilizationInfo.id == rndShipConstructionID)
                        return rnd;

            return null;
        }
        #endregion
    }
}