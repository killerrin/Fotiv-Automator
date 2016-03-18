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
                User = game.Players.Where(x => x.UserInfo.id == user.id).First(),
                Game = game
            });
        }

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
                User = game.Players.Where(x => x.UserInfo.id == user.id).First(),
                Civilizations = game.Civilizations,
                GameID = id
            });
        }

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