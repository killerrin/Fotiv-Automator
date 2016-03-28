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
using Fotiv_Automator.Infrastructure.CustomControllers;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class GameController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int gameID = -1)
        {
            Debug.WriteLine(string.Format("GET: Game Controller: Index - gameID={0}", gameID));

            List<DB_games> games = Database.Session.Query<DB_games>().ToList();
            games.RemoveAll(x => x.opened_to_public == false);

            return View(new IndexGames
            {
                Games = games
            });
        }

        [HttpGet]
        public override ActionResult View(int gameID = -1)
        {
            Debug.WriteLine(string.Format("GET: Game Controller: View - gameID={0}", gameID));

            Game game = GameState.QueryGame(gameID);
            if (game == null) return RedirectToRoute("home");

            DB_users user = Auth.User;
            return View(new ViewGame
            {
                GameID = game.Info.id,
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Game = game
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Game Controller: New Game"));
            return View(new NewGameForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(NewGameForm form)
        {
            DB_users user = Auth.User;
            if (user == null) return RedirectToRoute("login");

            Debug.WriteLine(string.Format("POST: Game Controller: New Game"));

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

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int gameID = -1)
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
        public ActionResult Edit(GameSettingsForm form)
        {
            Debug.WriteLine(string.Format("POST: Settings Controller: Index - gameID={0}", form.GameID));

            Game game = GameState.QueryGame(form.GameID);
            game.Info.name = form.Name;
            game.Info.description = form.Description;
            game.Info.opened_to_public = form.OpenedToPublic;
            Database.Session.Save(game.Info);
            Database.Session.Flush();

            ModelState.AddModelError("Updated", "The game has been successfully updated");
            return View(form);
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int gameID)
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