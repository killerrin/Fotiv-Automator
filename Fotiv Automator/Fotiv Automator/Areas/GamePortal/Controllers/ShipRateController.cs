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
    public class ShipRateController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? shipRateID = null)
        { 
            Debug.WriteLine(string.Format("GET: Ship Rate Controller: Index - shipRateID={0}", shipRateID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexShipRates
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                ShipRates = game.GameStatistics.ShipRatesRaw
            });
        }

        [HttpGet]
        public override ActionResult Show(int? shipRateID)
        {
            Debug.WriteLine(string.Format("GET: Ship Rate Controller: View - shipRateID={0}", shipRateID));
            if (shipRateID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewShipRate
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                ShipRate = game.GameStatistics.ShipRatesRaw.Find(x => x.id == shipRateID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Ship Rate Controller: New"));
            return View(new ShipRateForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(ShipRateForm form)
        {
            Debug.WriteLine(string.Format("POST: Ship Rate Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_ship_rates shipRate = new DB_ship_rates();
            shipRate.game_id = game.Info.id;
            shipRate.name = form.Name;
            shipRate.build_rate = form.BuildRate;
            Database.Session.Save(shipRate);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? shipRateID)
        {
            Debug.WriteLine(string.Format("GET: Ship Rate Controller: Edit - shipRateID={0}", shipRateID));
            var game = GameState.Game;

            var shipRate = game.GameStatistics.ShipRatesRaw.Find(x => x.id == shipRateID);
            return View(new ShipRateForm
            {
                ID = shipRate.id,
                Name = shipRate.name,
                BuildRate = shipRate.build_rate
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(ShipRateForm form, int? shipRateID)
        {
            Debug.WriteLine(string.Format("POST: Ship Rate Controller: Edit - shipRateID={0}", shipRateID));
            var game = GameState.Game;

            var shipRate = game.GameStatistics.ShipRatesRaw.Find(x => x.id == shipRateID);
            if (shipRate.game_id == null || shipRate.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            shipRate.name = form.Name;
            shipRate.build_rate = form.BuildRate;
            Database.Session.Update(shipRate);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? shipRateID)
        {
            Debug.WriteLine(string.Format("POST: Ship Rate Controller: Delete - shipRateID={0}", shipRateID));

            var shipRate = Database.Session.Load<DB_ship_rates>(shipRateID);
            if (shipRate == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (shipRate.game_id == null || shipRate.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(shipRate);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}