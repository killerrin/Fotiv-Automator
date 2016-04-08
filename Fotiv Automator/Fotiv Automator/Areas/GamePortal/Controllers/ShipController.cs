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
    public class ShipController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? shipID = null)
        { 
            Debug.WriteLine(string.Format("GET: Ship Controller: Index - shipID={0}", shipID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexShips
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Ships = game.GameStatistics.Ships
            });
        }

        [HttpGet]
        public override ActionResult Show(int? shipID)
        {
            Debug.WriteLine(string.Format("GET: Ship Controller: View - shipID={0}", shipID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewShip
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Ship = game.GameStatistics.Ships.Find(x => x.Info.id == shipID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Ship Controller: New"));

            var game = GameState.Game;
            var shipRates = new List<Checkbox>();
            shipRates.Add(new Checkbox(-1, "None", true));
            foreach (var shipRate in game.GameStatistics.ShipRatesRaw)
                shipRates.Add(new Checkbox(shipRate.id, shipRate.name, false));

            return View(new ShipForm
            {
                ShipRates = shipRates
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(ShipForm form)
        {
            Debug.WriteLine(string.Format("POST: Ship Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_ships ship = new DB_ships();
            ship.game_id = game.Info.id;
            ship.ship_rate_id = (form.SelectedShipRate == -1) ? null : form.SelectedShipRate;

            ship.name = form.Name;
            ship.description = form.Description;
            ship.rp_cost = form.RPCost;
            ship.base_health = form.BaseHealth;
            ship.base_regeneration = form.BaseRegeneration;
            ship.base_attack = form.BaseAttack;
            ship.base_special_attack = form.BaseSpecialAttack;
            ship.base_agility = form.BaseAgility;
            ship.maximum_fighters = form.MaximumFighters;
            ship.num_build = form.NumBuild;
            ship.gmnotes = form.GMNotes;
            Database.Session.Save(ship);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? shipID)
        {
            Debug.WriteLine(string.Format("GET: Ship Controller: Edit - shipID={0}", shipID));
            var game = GameState.Game;

            var ship = game.GameStatistics.ShipsRaw.Find(x => x.id == shipID);

            var shipRates = new List<Checkbox>();
            shipRates.Add(new Checkbox(-1, "None", false));
            foreach (var shipRate in game.GameStatistics.ShipRatesRaw)
                shipRates.Add(new Checkbox(shipRate.id, shipRate.name, shipRate.id == ship.ship_rate_id));

            var selected = shipRates.Where(x => x.IsChecked).ToList();
            if (selected.Count == 0) shipRates[0].IsChecked = true;

            return View(new ShipForm
            {
                ID = ship.id,

                Name = ship.name,
                Description = ship.description,
                RPCost = ship.rp_cost,

                BaseHealth = ship.base_health,
                BaseRegeneration = ship.base_regeneration,
                BaseAttack = ship.base_attack,
                BaseSpecialAttack = ship.base_special_attack,
                BaseAgility = ship.base_agility,
                MaximumFighters = ship.maximum_fighters,
                NumBuild = ship.num_build,

                GMNotes = ship.gmnotes,

                ShipRates = shipRates
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(ShipForm form, int? shipID)
        {
            Debug.WriteLine(string.Format("POST: Ship Controller: Edit - shipID={0}", shipID));
            var game = GameState.Game;

            var ship = game.GameStatistics.ShipsRaw.Find(x => x.id == shipID);
            if (ship.game_id == null || ship.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            ship.ship_rate_id = (form.SelectedShipRate == -1) ? null : form.SelectedShipRate; 

            ship.name = form.Name;
            ship.description = form.Description;
            ship.rp_cost = form.RPCost;
            ship.base_health = form.BaseHealth;
            ship.base_regeneration = form.BaseRegeneration;
            ship.base_attack = form.BaseAttack;
            ship.base_special_attack = form.BaseSpecialAttack;
            ship.base_agility = form.BaseAgility;
            ship.maximum_fighters = form.MaximumFighters;
            ship.num_build = form.NumBuild;
            ship.gmnotes = form.GMNotes;
            Database.Session.Update(ship);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? shipID)
        {
            Debug.WriteLine(string.Format("POST: Ship Controller: Delete - shipID={0}", shipID));

            var ship = Database.Session.Load<DB_ships>(shipID);
            if (ship == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (ship.game_id == null || ship.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(ship);
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}