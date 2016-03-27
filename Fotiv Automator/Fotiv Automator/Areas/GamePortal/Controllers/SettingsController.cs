using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class SettingsController : Controller
    {
        // GET: GamePortal/Settings
        [HttpGet]
        public ActionResult Index()
        {
            Debug.WriteLine(string.Format("GET: Settings Controller: Index"));

            Game game = GameState.QueryGame();
            if (game == null) return RedirectToRoute("home");

            User user = Auth.User;
            if (!game.IsPlayerGM(user.ID)) RedirectToRoute("game", new { gameID = game.Info.id });

            return View(new GameSettingsForm
            {
                GameID = game.Info.id,

                Name = game.Info.name,
                Description = game.Info.description,
                OpenedToPublic = game.Info.opened_to_public
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(GameSettingsForm form)
        {
            Debug.WriteLine(string.Format("POST: Settings Controller: Index - gameID={0}", form.GameID));

            Game game = GameState.QueryGame(form.GameID);
            game.Info.name = form.Name;
            game.Info.description = form.Description;
            game.Info.opened_to_public = form.OpenedToPublic;
            Database.Session.Save(game.Info);

            Database.Session.Flush();
            return RedirectToRoute("GameSettings");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int gameID)
        {
            Debug.WriteLine(string.Format("POST: Game Controller: Delete Game - gameID={0}", gameID));

            Game game = GameState.QueryGame();
            if (game == null) return HttpNotFound();
            if (game.Info.id != gameID) return HttpNotFound();

            User user = Auth.User;
            if (!game.IsPlayerGM(user.ID)) return HttpNotFound();

            Database.Session.Delete(game.Info);
            Database.Session.Flush();

            return RedirectToRoute("home");
        }
    }
}