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
    public class WormholeController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? wormholeID)
        {
            Debug.WriteLine($"GET: Wormhole Controller: View - wormholeID={wormholeID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewWormhole
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Wormhole = game.Sector.WormholeFromID(wormholeID.Value)
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? starsystemID = null)
        {
            Debug.WriteLine($"GET: Wormhole Controller: New - starsystemID={starsystemID}");
            var game = GameState.Game;

            return View(new WormholeForm
            {
                StarsystemID = starsystemID.Value,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(WormholeForm form, int? starsystemID = null)
        {
            Debug.WriteLine($"POST: Wormhole Controller: New - starsystemID={form.StarsystemID} HexX={form.HexX} HexY={form.HexY}");
            var game = GameState.Game;

            var systemTwo = game.Sector.StarsystemFromHex(new Fotiv_Automator.Models.Tools.HexCoordinate(form.HexX, form.HexY));
            if (systemTwo == null) ModelState.AddModelError("Hex Code", "Star System does not exist");

            if (!ModelState.IsValid)
                return View(form);

            Debug.WriteLine($"POST: Wormhole Controller: New - systemTwo.ID={systemTwo.ID} systemTwo.Hex.X={systemTwo.HexCode.X} systemTwo.Hex.Y={systemTwo.HexCode.Y}");

            DB_wormholes wormhole = new DB_wormholes();
            wormhole.game_id = game.ID;
            wormhole.system_id_one = form.StarsystemID;
            wormhole.system_id_two = systemTwo.ID;
            wormhole.gmnotes = form.GMNotes;
            Database.Session.Save(wormhole);
            
            Database.Session.Flush();
            return RedirectToRoute("StarMap");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? wormholeID)
        {
            Debug.WriteLine($"GET: Wormhole Controller: Edit - starID={wormholeID}");

            var game = GameState.Game;
            var wormhole = game.Sector.WormholeFromID(wormholeID.Value);


            return View(new WormholeForm
            {
                ID = wormhole.Info.id,
                StarsystemID = wormhole.Info.system_id_one,
                HexX = wormhole.SystemTwo.HexCode.X,
                HexY = wormhole.SystemTwo.HexCode.Y,
                GMNotes = wormhole.Info.gmnotes,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(WormholeForm form, int? wormholeID)
        {
            Debug.WriteLine($"POST: Wormhole Controller: Edit - wormholeID={wormholeID} HexX={form.HexX} HexY={form.HexY}");
            var game = GameState.Game;

            var wormhole = game.Sector.WormholeFromID(form.ID.Value).Info;
            if (wormhole.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var systemTwo = game.Sector.StarsystemFromHex(new Fotiv_Automator.Models.Tools.HexCoordinate(form.HexX, form.HexY));
            if (systemTwo == null) ModelState.AddModelError("Hex Code", "Star System does not exist");

            if (!ModelState.IsValid)
                return View(form);

            wormhole.system_id_one = form.StarsystemID;
            wormhole.system_id_two = systemTwo.ID;
            wormhole.gmnotes = form.GMNotes;
            Database.Session.Update(wormhole);

            Database.Session.Flush();
            return RedirectToRoute("StarMap");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? wormholeID)
        {
            Debug.WriteLine($"POST: Wormhole Controller: Delete - wormholeID={wormholeID}");

            var wormhole = Database.Session.Load<DB_wormholes>(wormholeID.Value);
            if (wormhole == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (wormhole.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(wormhole);

            Database.Session.Flush();
            return RedirectToRoute("StarMap");
        }
    }
}