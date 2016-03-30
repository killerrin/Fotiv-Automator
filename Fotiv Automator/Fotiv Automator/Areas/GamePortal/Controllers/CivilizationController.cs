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

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class CivilizationController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int gameID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Civilization Controller: Index - gameID={0}", gameID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame(gameID);
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexCivilizations
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilizations = game.Civilizations,
                GameID = gameID
            });
        }

        [HttpGet]
        public override ActionResult View(int civilizationID = -1)
        {
            if (civilizationID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Civilization Controller: View Civilization - civilizationID={0}", civilizationID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewCivilization
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilization = game.Civilizations.Find(x => x.Info.id == civilizationID),
                GameID = game.Info.id
            });
        }

        #region New Civilization
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: New Civilization"));

            var game = GameState.Game;

            var players = new List<Checkbox>();
            foreach (var player in game.Players)
                players.Add(new Checkbox(player.User.ID, player.User.Username, false));

            var civilizationTraits = new List<Checkbox>();
            foreach (var trait in game.GameStatistics.CivilizationTraits)
                civilizationTraits.Add(new Checkbox(trait.id, trait.name, false));

            return View(new CivilizationForm
            {
                Players = players,
                CivilizationTraits = civilizationTraits
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(CivilizationForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: New Civilization - gameID={0}", GameState.GameID));

            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_civilization civilization = new DB_civilization();

            var selectedTraits = form.CivilizationTraits.Where(x => x.IsChecked).ToList();
            if (selectedTraits.Count > 0)
                civilization.civilization_traits_1_id = selectedTraits[0].ID;
            if (selectedTraits.Count > 1)
                civilization.civilization_traits_2_id = selectedTraits[1].ID;
            if (selectedTraits.Count > 2)
                civilization.civilization_traits_3_id = selectedTraits[2].ID;

            civilization.name = form.Name;
            civilization.colour = form.Colour;
            civilization.rp = form.RP;
            civilization.gmnotes = form.GMNotes;
            Database.Session.Save(civilization);

            DB_game_civilizations gameCivilization = new DB_game_civilizations();
            gameCivilization.civilization_id = civilization.id;
            gameCivilization.game_id = game.Info.id;
            Database.Session.Save(gameCivilization);

            foreach (var player in form.Players)
            {
                if (player.IsChecked)
                {
                    DB_user_civilizations userCivilization = new DB_user_civilizations();
                    userCivilization.user_id = player.ID;
                    userCivilization.civilization_id = civilization.id;
                    Database.Session.Save(userCivilization);
                }
            }
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit Civilization
        [HttpGet]
        public override ActionResult Edit(int civilizationID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: Edit Civilization - civilizationID={0}", civilizationID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilization = game.Civilizations.Find(x => x.Info.id == civilizationID);

            var players = new List<Checkbox>();
            foreach (var player in game.Players)
                players.Add(new Checkbox(player.User.ID, player.User.Username, civilization.PlayerOwnsCivilization(player.User.ID)));

            var civilizationTraits = new List<Checkbox>();
            foreach (var trait in game.GameStatistics.CivilizationTraits)
                civilizationTraits.Add(new Checkbox(trait.id, trait.name, civilization.CivilizationHasTrait(trait.id)));

            return View(new CivilizationForm
            {
                CivilizationID = civilizationID,

                Name = civilization.Info.name,
                Colour = civilization.Info.colour,
                RP = civilization.Info.rp,

                GMNotes = civilization.Info.gmnotes,

                Players = players,
                CivilizationTraits = civilizationTraits
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CivilizationForm form, int civilizationID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: Edit Civilization - civilizationID={0}", civilizationID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilization = game.Civilizations.Find(x => x.Info.id == civilizationID);
            form.CivilizationID = civilization.Info.id;

            var selectedTraits = form.CivilizationTraits.Where(x => x.IsChecked).ToList();
            if (selectedTraits.Count > 0)
                civilization.Info.civilization_traits_1_id = selectedTraits[0].ID;
            if (selectedTraits.Count > 1)
                civilization.Info.civilization_traits_2_id = selectedTraits[1].ID;
            if (selectedTraits.Count > 2)
                civilization.Info.civilization_traits_3_id = selectedTraits[2].ID;

            civilization.Info.name = form.Name;
            civilization.Info.colour = form.Colour;
            civilization.Info.rp = form.RP;
            civilization.Info.gmnotes = form.GMNotes;
            Database.Session.Update(civilization.Info);

            var userCivs = Database.Session.Query<DB_user_civilizations>()
                .Where(x => x.civilization_id == civilization.Info.id)
                .ToList();

            var checkedOwners = form.Players
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_user_civilizations> toRemove = new List<DB_user_civilizations>();
            List<Checkbox> toAdd = new List<Checkbox>();

            // First determine what to remove
            foreach (var userCiv in userCivs)
            {
                bool foundMatch = false;
                foreach (var checkBox in checkedOwners)
                {
                    // Player is already an Owner of this Civilization
                    if (userCiv.user_id == checkBox.ID)
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
                    if (checkBox.ID == userCiv.user_id)
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
                Database.Session.Save(new DB_user_civilizations(add.ID, civilization.Info.id));

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int civilizationID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: Delete Civilization - civilizationID={0}", civilizationID));

            var civilization = Database.Session.Load<DB_civilization>(civilizationID);
            if (civilization == null)
                return HttpNotFound();

            Database.Session.Delete(civilization);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}