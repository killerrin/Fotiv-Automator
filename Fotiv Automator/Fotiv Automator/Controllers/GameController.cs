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
            Debug.WriteLine(string.Format("GET: Game Controller: Index - id={0}", id));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if (game == null)
                game = QueryGame(id);

            return View(new GameIndex
            {
                GameID = game.Info.id,
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Game = game
            });
        }

        #region GM Settings
        [HttpGet]
        public ActionResult GameSettings(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Game Controller: GameSettings - id={0}", id));

            Game game = QueryGame(id);
            return View(new GameSettingsForm
            {
                GameID = id,

                Name = game.Info.name,
                Description = game.Info.description,
                OpenedToPublic = game.Info.opened_to_public
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult GameSettings(GameSettingsForm form)
        {
            Debug.WriteLine(string.Format("POST: Game Controller: GameSettings - id={0}", form.GameID));

            Game game = QueryGame(form.GameID);
            return RedirectToRoute("GameSettings", new { id = form.GameID });
        }
        #endregion


        [HttpGet]
        public ActionResult Civilizations(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Game Controller: Civilizations - id={0}", id));

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
            Debug.WriteLine(string.Format("GET: Game Controller: NewGame"));
            return View(new NewGameForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewGame(NewGameForm form)
        {
            DB_users user = Auth.User;
            if (user == null)
                return RedirectToRoute("login");

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

        #region New Civilization
        [HttpGet]
        public ActionResult NewCivilization(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Game Controller: NewCivilization - id={0}", id));

            return View(new NewUpdateCivilizationForm { GameID = id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewCivilization(NewUpdateCivilizationForm form)
        {
            if (form.GameID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("POST: Game Controller: NewCivilization - id={0}", form.GameID));

            DB_civilization civilization = new DB_civilization();
            civilization.name = form.Name;
            civilization.colour = form.Colour;
            civilization.rp = form.RP;
            civilization.notes = form.Notes;
            civilization.gmnotes = form.GMNotes;
            Database.Session.Save(civilization);

            DB_game_civilizations gameCivilization = new DB_game_civilizations();
            gameCivilization.civilization_id = civilization.id;
            gameCivilization.game_id = form.GameID;
            Database.Session.Save(gameCivilization);

            Database.Session.Flush();
            return RedirectToRoute("game", new { id = form.GameID });
        }
        #endregion

        private Game QueryGame(int id)
        {
            Debug.WriteLine(string.Format("Game Controller: Querying New Game - id={0}", id));
            DB_games db_game = Database.Session.Query<DB_games>()
                .Where(x => x.id == id)
                .First();

            Game game = new Game(db_game);
            GameState.Set(game);

            return game;
        }
    }
}