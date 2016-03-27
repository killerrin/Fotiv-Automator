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
using Fotiv_Automator.Models;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public ActionResult Index(int gameID = -1)
        {
            Debug.WriteLine(string.Format("GET: Game Controller: Index - gameID={0}", gameID));

            Game game = GameState.QueryGame(gameID);
            if (game == null) return RedirectToRoute("home");

            DB_users user = Auth.User;
            return View(new IndexGame
            {
                GameID = game.Info.id,
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Game = game
            });
        }

        #region New Game
        [HttpGet]
        public ActionResult NewGame()
        {
            Debug.WriteLine(string.Format("GET: Game Controller: NewGame"));
            return View(new NewGameForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewGame(NewGameForm form)
        {
            DB_users user = Auth.User;
            if (user == null) return RedirectToRoute("login");

            Debug.WriteLine(string.Format("POST: Game Controller: NewGame"));

            DB_games newGame = new DB_games();
            newGame.name = form.Name;
            newGame.description = form.Description;
            newGame.opened_to_public = form.OpenedToPublic;
            Database.Session.Save(newGame);

            DB_game_users gameUser = new DB_game_users();
            gameUser.user_id = user.id;
            gameUser.game_id = newGame.id;
            gameUser.is_gm = true;
            Database.Session.Save(gameUser);

            Database.Session.Flush();
            return RedirectToRoute("home");
        }
        #endregion
    }
}