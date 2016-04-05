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
    public class RnDShipConstructionController : NewViewEditDeleteController
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
                CivilizationName = civilization.Info.name
            });
        }

        [HttpGet]
        public override ActionResult View(int? rndShipConstructionID)
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

            DB_civilization_research research = new DB_civilization_research();
            research.build_percentage = form.BuildPercentage;
            research.research_id = form.SelectedResearched.Value;
            research.civilization_id = form.CivilizationID.Value;
            Database.Session.Save(research);

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
            var shipsCheckBoxes = GameState.Game.GameStatistics.Ships.Select(x => new Checkbox(x.Info.id, x.Info.name, x.Info.id == ship.CivilizationInfo.ship_id)).ToList();

            return View(new RnDShipConstructionForm
            {
                ID = ship.CivilizationInfo.id,
                CivilizationID = ship.CivilizationInfo.civilization_id,
                BuildPercentage = ship.CivilizationInfo.build_percentage,

                Ships = shipsCheckBoxes,
                SelectedShip = shipsCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RnDShipConstructionForm form, int? rndShipConstructionID)
        {
            Debug.WriteLine($"POST: R&D Research Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_research research = FindRNDShip(rndShipConstructionID).CivilizationInfo;
            if (!RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            research.build_percentage = form.BuildPercentage;
            research.research_id = form.SelectedResearched.Value;
            research.civilization_id = form.CivilizationID.Value;
            Database.Session.Update(research);

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
            if (!game.GetCivilization(ship.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
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