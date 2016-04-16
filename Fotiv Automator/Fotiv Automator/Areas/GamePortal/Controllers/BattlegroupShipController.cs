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
    public class BattlegroupShipController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: Battlegroup Ship Controller: Index - civilizationID={civilizationID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            if (civilizationID == null) return RedirectToRoute("home");
            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new IndexBattlegroupShips
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Battlegroups = civilization.Assets.BattlegroupShips,
                CivilizationID = civilization.Info.id,
                CivilizationName = civilization.Info.name,

                PlayerOwnsCivilization = civilization.PlayerOwnsCivilization(user.id)
            });
        }

        [HttpGet]
        public override ActionResult Show(int? battlegroupID)
        {
            Debug.WriteLine($"GET: Battlegroup Ship Controller: View - {nameof(battlegroupID)}={battlegroupID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var battlegroup = FindBattlegroup(battlegroupID);

            return View(new ViewBattlegroupShip
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Battlegroup = battlegroup,
                PlayerOwnsCivilization = game.GetCivilization(battlegroup.Info.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: Battlegroup Ship Controller: New");

            Game game = GameState.Game;
            if (civilizationID == null) return RedirectToRoute("home");
            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new BattlegroupShipForm
            {
                CivilizationID = civilizationID,
                UnassignedShips = civilization.Assets.CompletedShips
                    .Where(x => x.CivilizationInfo.ship_battlegroup_id == null)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.Ship.Info.name}", false))
                    .ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(BattlegroupShipForm form)
        {
            Debug.WriteLine($"POST: Battlegroup Ship Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var civilization = game.GetCivilization(form.CivilizationID.Value);
            if (!civilization.PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var system = game.Sector.StarsystemFromHex(form.HexX, form.HexY);
            if (system != null && system.Info.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            // Create the Battlegroup and save it to the DB
            DB_ship_battlegroups battlegroup = new DB_ship_battlegroups();
            battlegroup.game_id = game.ID;
            battlegroup.civilization_id = civilization.ID;
            battlegroup.starsystem_id = system.ID;
            battlegroup.name = form.Name;
            battlegroup.gmnotes = form.GMNotes;
            Database.Session.Save(battlegroup);

            // Add in all the checked Ships
            foreach (var shipCheckbox in form.UnassignedShips)
            {
                if (shipCheckbox.IsChecked)
                {
                    var ship = FindRNDShip(shipCheckbox.ID);
                    ship.CivilizationInfo.ship_battlegroup_id = battlegroup.id;
                    Database.Session.Update(ship.CivilizationInfo);
                }
            }

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? battlegroupID)
        {
            Debug.WriteLine($"GET: Battlegroup Ship Controller: Edit - {nameof(battlegroupID)}={battlegroupID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var battlegroup = FindBattlegroup(battlegroupID);
            var civilization = game.GetCivilization(battlegroup.Info.civilization_id);
            var system = game.Sector.StarsystemFromID(battlegroup.Info.starsystem_id);

            return View(new BattlegroupShipForm
            {
                ID = battlegroup.Info.id,
                CivilizationID = civilization.ID,

                HexX = system.HexCode.X,
                HexY = system.HexCode.Y,

                Name = battlegroup.Info.name,
                GMNotes = battlegroup.Info.gmnotes,

                UnassignedShips = civilization.Assets.CompletedShips
                    .Where(x => x.CivilizationInfo.ship_battlegroup_id == null)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.Ship.Info.name}", false))
                    .ToList(),
                BattlegroupShips = battlegroup.Ships
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.Ship.Info.name}", true))
                    .ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(BattlegroupShipForm form, int? battlegroupID)
        {
            Debug.WriteLine($"POST: Battlegroup Ship Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var civilization = game.GetCivilization(form.CivilizationID.Value);
            if (!civilization.PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var system = game.Sector.StarsystemFromHex(form.HexX, form.HexY);
            if (system != null && system.Info.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var battlegroup = FindBattlegroup(battlegroupID);
            if (battlegroup != null && battlegroup.Info.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            // Update the Battlegroup and Update it in the DB
            battlegroup.Info.starsystem_id = system.ID;
            battlegroup.Info.name = form.Name;
            if (RequireGMAdminAttribute.IsGMOrAdmin()) battlegroup.Info.gmnotes = form.GMNotes;
            Database.Session.Update(battlegroup.Info);

            // Add in all of the newly checked Ships
            foreach (var shipCheckbox in form.UnassignedShips)
            {
                if (shipCheckbox.IsChecked)
                {
                    var ship = FindRNDShip(shipCheckbox.ID);
                    ship.CivilizationInfo.ship_battlegroup_id = battlegroup.ID;
                    Database.Session.Update(ship.CivilizationInfo);
                }
            }

            // Remove all the Ships no longer checked
            foreach (var shipCheckbox in form.BattlegroupShips)
            {
                if (!shipCheckbox.IsChecked)
                {
                    var ship = FindRNDShip(shipCheckbox.ID);
                    ship.CivilizationInfo.ship_battlegroup_id = null;
                    Database.Session.Update(ship.CivilizationInfo);
                }
            }

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? battlegroupID)
        {
            Debug.WriteLine($"POST: Battlegroup Ship Controller: Delete - {nameof(battlegroupID)}={battlegroupID}");

            var battlegroup = Database.Session.Load<DB_ship_battlegroups>(battlegroupID);
            if (battlegroup == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(battlegroup.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                battlegroup.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            int tempCivilizationID = battlegroup.civilization_id;
            Database.Session.Delete(battlegroup);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = tempCivilizationID });
        }

        #region Tools
        private BattlegroupShip FindBattlegroup(int? battlegroupID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var bg in civilization.Assets.BattlegroupShips)
                    if (bg?.Info.id == battlegroupID)
                        return bg;

            return null;
        }

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