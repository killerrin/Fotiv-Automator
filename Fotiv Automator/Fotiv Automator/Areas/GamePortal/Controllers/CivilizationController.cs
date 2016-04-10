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
    public class CivilizationController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine(string.Format("GET: Civilization Controller: Index - gameID={0}", gameID));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            game.QueryAndConnectCivilizations();

            return View(new IndexCivilizations
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilizations = game.Civilizations,
                GameID = game.ID
            });
        }

        [HttpGet]
        public override ActionResult Show(int? civilizationID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: View Civilization - civilizationID={0}", civilizationID));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            game.QueryAndConnectCivilizations();

            return View(new ViewCivilization
            {
                GameID = game.Info.id,
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Civilization = game.Civilizations.Find(x => x.Info.id == civilizationID)
            });
        }

        #region New Civilization
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: New Civilization"));
            var game = GameState.Game;

            var players = new List<Checkbox>();
            foreach (var player in game.Players)
                players.Add(new Checkbox(player.User.ID, player.User.Username, false));

            var civilizationTraits = new List<Checkbox>();
            foreach (var trait in game.GameStatistics.CivilizationTraits)
                civilizationTraits.Add(new Checkbox(trait.id, trait.name, false));

            var metCivilizations = new List<Checkbox>();
            foreach (var allCivilization in game.Civilizations)
                metCivilizations.Add(new Checkbox(allCivilization.Info.id, allCivilization.Info.name, false));

            var techLevels = new List<Checkbox>();
            techLevels.Add(new Checkbox(-1, "None", true));
            foreach (var techLevel in game.GameStatistics.TechLevels)
                techLevels.Add(new Checkbox(techLevel.id, techLevel.name, false));

            return View(new CivilizationForm
            {
                Players = players,
                CivilizationTraits = civilizationTraits,
                MetCivilizations = metCivilizations,
                TechLevels = techLevels
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(CivilizationForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: New Civilization - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_civilization civilization = new DB_civilization();
            civilization.game_id = game.Info.id;

            var selectedTraits = form.CivilizationTraits.Where(x => x.IsChecked).ToList();
            if (selectedTraits.Count > 0)
                civilization.civilization_traits_1_id = selectedTraits[0].ID;
            if (selectedTraits.Count > 1)
                civilization.civilization_traits_2_id = selectedTraits[1].ID;
            if (selectedTraits.Count > 2)
                civilization.civilization_traits_3_id = selectedTraits[2].ID;

            civilization.tech_level_id = (form.SelectedTechLevel == -1) ? null : form.SelectedTechLevel;

            civilization.name = form.Name;
            civilization.colour = form.Colour;
            civilization.rp = form.RP;
            civilization.gmnotes = form.GMNotes;
            Database.Session.Save(civilization);

            foreach (var player in form.Players)
            {
                if (player.IsChecked)
                {
                    DB_user_civilizations userCivilization = new DB_user_civilizations();
                    userCivilization.game_id = game.ID;
                    userCivilization.user_id = player.ID;
                    userCivilization.civilization_id = civilization.id;
                    Database.Session.Save(userCivilization);
                }
            }

            foreach (var civilizationMet in form.MetCivilizations)
            {
                if (civilizationMet.IsChecked)
                {
                    DB_civilization_met dbCivilizationMet = new DB_civilization_met();
                    dbCivilizationMet.game_id = game.ID;
                    dbCivilizationMet.civilization_id1 = civilization.id;
                    dbCivilizationMet.civilization_id2 = civilizationMet.ID;
                    Database.Session.Save(dbCivilizationMet);
                }
            }
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit Civilization
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? civilizationID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Controller: Edit Civilization - civilizationID={0}", civilizationID));
            var game = GameState.Game;

            var civilization = game.Civilizations.Find(x => x.Info.id == civilizationID);

            var players = new List<Checkbox>();
            foreach (var player in game.Players)
                players.Add(new Checkbox(player.User.ID, player.User.Username, civilization.PlayerOwnsCivilization(player.User.ID)));

            var civilizationTraits = new List<Checkbox>();
            foreach (var trait in game.GameStatistics.CivilizationTraits)
                civilizationTraits.Add(new Checkbox(trait.id, trait.name, civilization.CivilizationHasTrait(trait.id)));

            var metCivilizations = new List<Checkbox>();
            var allCivilizationsButThis = game.Civilizations.Where(x => x.ID != civilization.ID).ToList();
            foreach (var allCivilization in allCivilizationsButThis)
                metCivilizations.Add(new Checkbox(allCivilization.Info.id, allCivilization.Info.name, civilization.HasMetCivilization(allCivilization.ID)));

            var techLevels = new List<Checkbox>();
            techLevels.Add(new Checkbox(-1, "None", true));
            foreach (var techLevel in game.GameStatistics.TechLevels)
                techLevels.Add(new Checkbox(techLevel.id, techLevel.name, techLevel.id == civilization.Info.tech_level_id));

            var selected = techLevels.Where(x => x.IsChecked).ToList();
            if (selected.Count == 0) techLevels[0].IsChecked = true;

            return View(new CivilizationForm
            {
                ID = civilizationID,

                Name = civilization.Info.name,
                Colour = civilization.Info.colour,
                RP = civilization.Info.rp,

                GMNotes = civilization.Info.gmnotes,

                Players = players,
                CivilizationTraits = civilizationTraits,
                MetCivilizations = metCivilizations,
                TechLevels = techLevels
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(CivilizationForm form, int? civilizationID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: Edit Civilization - civilizationID={0}", civilizationID));
            var game = GameState.Game;

            var civilization = game.Civilizations.Find(x => x.Info.id == civilizationID);
            if (civilization.Info.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var selectedTraits = form.CivilizationTraits.Where(x => x.IsChecked).ToList();
            if (selectedTraits.Count > 0)
                civilization.Info.civilization_traits_1_id = selectedTraits[0].ID;
            if (selectedTraits.Count > 1)
                civilization.Info.civilization_traits_2_id = selectedTraits[1].ID;
            if (selectedTraits.Count > 2)
                civilization.Info.civilization_traits_3_id = selectedTraits[2].ID;

            civilization.Info.tech_level_id = (form.SelectedTechLevel == -1) ? null : form.SelectedTechLevel;

            civilization.Info.name = form.Name;
            civilization.Info.colour = form.Colour;
            civilization.Info.rp = form.RP;
            civilization.Info.gmnotes = form.GMNotes;
            Database.Session.Update(civilization.Info);

            #region User Civilizations
            var userCivs = Database.Session.Query<DB_user_civilizations>()
                .Where(x => x.civilization_id == civilization.Info.id)
                .ToList();

            var checkedOwners = form.Players
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_user_civilizations> ownerToRemove = new List<DB_user_civilizations>();
            List<Checkbox> ownerToAdd = new List<Checkbox>();

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
                    ownerToRemove.Add(userCiv);
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
                    ownerToAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in ownerToRemove)
                Database.Session.Delete(remove);
            foreach (var add in ownerToAdd)
                Database.Session.Save(new DB_user_civilizations(add.ID, civilization.Info.id, game.ID));
            #endregion

            #region Met Civilizations
            var metCivilizations = Database.Session.Query<DB_civilization_met>()
                .Where(x => x.civilization_id1 == civilization.Info.id || x.civilization_id2 == civilization.Info.id)
                .ToList();

            var checkedMetCivilizations = form.MetCivilizations
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_civilization_met> metCivilizationToRemove = new List<DB_civilization_met>();
            List<Checkbox> metCivilizationToAdd = new List<Checkbox>();

            // First determine what to remove
            foreach (var metCivilization in metCivilizations)
            {
                bool foundMatch = false;
                foreach (var checkBox in checkedMetCivilizations)
                {
                    // Player has already met this Civilization
                    if (metCivilization.civilization_id1 == checkBox.ID ||
                        metCivilization.civilization_id2 == checkBox.ID)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // No longer an met this Civilization
                if (!foundMatch)
                    metCivilizationToRemove.Add(metCivilization);
            }

            // Next determine what is new
            foreach (var checkBox in checkedMetCivilizations)
            {
                bool foundMatch = false;
                foreach (var metCivilization in metCivilizations)
                {
                    // Player has already met this civilization
                    if (checkBox.ID == metCivilization.civilization_id1 ||
                        checkBox.ID == metCivilization.civilization_id2)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // We have a new owner!
                if (!foundMatch)
                    metCivilizationToAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in metCivilizationToRemove)
                Database.Session.Delete(remove);
            foreach (var add in metCivilizationToAdd)
                Database.Session.Save(new DB_civilization_met(civilization.Info.id, add.ID, game.ID));
            #endregion

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? civilizationID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Controller: Delete Civilization - civilizationID={0}", civilizationID));

            var civilization = Database.Session.Load<DB_civilization>(civilizationID);
            if (civilization == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (civilization.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(civilization);
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}