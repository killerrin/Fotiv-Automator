using Fotiv_Automator.Models.DatabaseMaps;
using Fotiv_Automator.Models.Game;
using Fotiv_Automator.ViewModels;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public ActionResult Index(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("Game Controller: Index - id={0}", id));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if (game == null)
                game = QueryGame(id);

            return View(new GameIndex
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Game = game
            });
        }

        [HttpGet]
        public ActionResult Civilizations(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("Game Controller: Civilizations"));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if (game == null)
                game = QueryGame(id);

            return View(new GameCivilizations
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilizations = game.Civilizations,
                GameID = id
            });
        }

        #region New Game
        [HttpGet]
        public ActionResult NewGame()
        {
            return View(new NewGameForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewGame(NewGameForm form)
        {
            DB_users user = Auth.User;
            if (user == null)
                return RedirectToRoute("login");

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

        #region New Civilization
        [HttpGet]
        public ActionResult NewCivilization(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            return View();
        }
        #endregion

        private Game QueryGame(int id)
        {
            Debug.WriteLine("Creating New Game");
            DB_games db_game = Database.Session.Query<DB_games>()
                .Where(x => x.id == id)
                .First();

            Game game = new Game(db_game);
            GameState.Game = game;

            return game;
        }
    }
}