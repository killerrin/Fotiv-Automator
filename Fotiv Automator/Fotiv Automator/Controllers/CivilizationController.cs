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
    public class CivilizationController : Controller
    {
        [HttpGet]
        public ActionResult ListCivilizations(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Civilization Controller: Civilizations - id={0}", id));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame(id);
            //Game game = GameState.Game;
            //if (game == null)
            //    game = GameState.QueryGame(id);

            return View(new GameCivilizations
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilizations = game.Civilizations,
                GameID = id
            });
        }

        [HttpGet]
        public ActionResult ViewCivilization(int id = -1)
        {
            if (id == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Civilization Controller: View Civilization - id={0}", id));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewCivilization
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilization = game.Civilizations.Find(x => x.Info.id == id),
                GameID = game.Info.id
            });
        }

        #region New Civilization
        [HttpGet]
        public ActionResult NewCivilization()
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: New Civilization"));

            var game = GameState.Game;
            var players = new List<PlayerCheckbox>();
            foreach (var player in game.Players)
                players.Add(new PlayerCheckbox(player.User.ID, player.User.Username, false));

            return View(new NewUpdateCivilizationForm
            {
                Game = game,
                Players = players
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewCivilization(NewUpdateCivilizationForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: New Civilization - id={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;
            form.Game = game;

            DB_civilization civilization = new DB_civilization();
            civilization.name = form.Name;
            civilization.colour = form.Colour;
            civilization.rp = form.RP;
            civilization.notes = form.Notes;
            civilization.gmnotes = form.GMNotes;
            Database.Session.Save(civilization);

            DB_game_civilizations gameCivilization = new DB_game_civilizations();
            gameCivilization.civilization_id = civilization.id;
            gameCivilization.game_id = form.Game.Info.id;
            Database.Session.Save(gameCivilization);

            foreach (var player in form.Players)
            {
                if (player.IsChecked)
                {
                    DB_user_civilizations userCivilization = new DB_user_civilizations();
                    userCivilization.user_id = player.PlayerID;
                    userCivilization.civilization_id = civilization.id;
                    Database.Session.Save(userCivilization);
                }
            }
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { id = form.Game.Info.id });
        }
        #endregion

        #region Edit Civilization
        [HttpGet]
        public ActionResult EditCivilization(int id)
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: Edit Civilization - id={0}", id));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilization = game.Civilizations.Find(x => x.Info.id == id);

            var players = new List<PlayerCheckbox>();
            foreach (var player in game.Players)
                players.Add(new PlayerCheckbox(player.User.ID, player.User.Username, civilization.PlayerOwnsCivilization(player.User.ID)));

            return View(new NewUpdateCivilizationForm
            {
                Game = game,
                CivilizationID = id,

                Name = civilization.Info.name,
                Colour = civilization.Info.colour,
                RP = civilization.Info.rp,

                Notes = civilization.Info.notes,
                GMNotes = civilization.Info.gmnotes,

                Players = players
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditCivilization(NewUpdateCivilizationForm form, int id)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: Edit Civilization - id={0}", id));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilization = game.Civilizations.Find(x => x.Info.id == id);
            form.CivilizationID = civilization.Info.id;
            form.Game = game;

            civilization.Info.name = form.Name;
            civilization.Info.colour = form.Colour;
            civilization.Info.rp = form.RP;
            civilization.Info.notes = form.Notes;
            civilization.Info.gmnotes = form.GMNotes;
            Database.Session.Update(civilization.Info);

            var userCivs = Database.Session.Query<DB_user_civilizations>()
                .Where(x => x.civilization_id == civilization.Info.id)
                .ToList();

            var checkedOwners = form.Players
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_user_civilizations> toRemove = new List<DB_user_civilizations>();
            List<PlayerCheckbox> toAdd = new List<PlayerCheckbox>();

            // First determine what to remove
            foreach (var userCiv in userCivs)
            {
                bool foundMatch = false;
                foreach (var checkBox in checkedOwners)
                {
                    // Player is already an Owner of this Civilization
                    if (userCiv.user_id == checkBox.PlayerID)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // No longer an Owner of this Civilization
                if (!foundMatch)
                    toRemove.Add(userCiv);
            }

            // Next determine what is new
            foreach (var checkBox in checkedOwners)
            {
                bool foundMatch = false;
                foreach (var userCiv in userCivs)
                {
                    // Player is already an Owner of this civilization
                    if (checkBox.PlayerID == userCiv.user_id)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // We have a new owner!
                if (!foundMatch)
                    toAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in toRemove)
                Database.Session.Delete(remove);
            foreach (var add in toAdd)
                Database.Session.Save(new DB_user_civilizations(add.PlayerID, civilization.Info.id));

            Database.Session.Flush();
            return RedirectToRoute("game", new { id = game.Info.id });
        }
        #endregion
    }
}