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
    public class StarsystemController : DataController
    {
        [HttpGet]
        public  ActionResult Show(int gameID, int? starsystemID)
        {
            Debug.WriteLine($"GET: Star Controller: View - starsystemID={starsystemID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            var system = game.Sector.StarsystemFromID(starsystemID.Value);
            return View(new ViewStarSystem
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                System = system,
            });
        }

        #region Edit
        [HttpGet, RequireGMAdmin]
        public  ActionResult Edit(int gameID, int? starsystemID)
        {
            Debug.WriteLine($"GET: Star Controller: Edit - starsystemID={starsystemID}");

            var game = GameState.Game;
            var starsystem = game.Sector.StarsystemFromID(starsystemID.Value);

            var civilizations = new List<Checkbox>();
            foreach (var civilization in game.Civilizations)
                civilizations.Add(new Checkbox(civilization.ID, civilization.Info.name, civilization.HasVisitedSystem(starsystem.ID)));

            return View(new StarsystemForm
            {
                ID = starsystem.ID,

                GMNotes = starsystem.Info.gmnotes,
                CivilizationVisited = civilizations,
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StarsystemForm form)
        {
            Debug.WriteLine($"POST: Star Controller: Edit - starsystemID={form.ID}");
            var game = GameState.Game;

            var starsystem = game.Sector.StarsystemFromID(form.ID.Value);
            if (starsystem.Info.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            starsystem.Info.gmnotes = form.GMNotes;
            Database.Session.Update(starsystem.Info);

            #region Civilizations Visited Starsystem
            var visitedStarsystems = Database.Session.Query<DB_visited_starsystems>()
                .Where(x => x.starsystem_id == starsystem.Info.id)
                .ToList();

            var checkedCivilizations = form.CivilizationVisited
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_visited_starsystems> civilizationToRemove = new List<DB_visited_starsystems>();
            List<Checkbox> civilizationToAdd = new List<Checkbox>();

            // First determine what to remove
            foreach (var visitedSystem in visitedStarsystems)
            {
                bool foundMatch = false;
                foreach (var checkBox in checkedCivilizations)
                {
                    // Civilization has already Visited this Civilization
                    if (visitedSystem.civilization_id == checkBox.ID)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // No longer an Owner of this Civilization
                if (!foundMatch)
                    civilizationToRemove.Add(visitedSystem);
            }

            // Next determine what is new
            foreach (var checkBox in checkedCivilizations)
            {
                bool foundMatch = false;
                foreach (var visitedSystem in visitedStarsystems)
                {
                    // Civilization has already Visited this Civilization
                    if (checkBox.ID == visitedSystem.civilization_id)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // We have a new owner!
                if (!foundMatch)
                    civilizationToAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in civilizationToRemove)
                Database.Session.Delete(remove);
            foreach (var add in civilizationToAdd)
                Database.Session.Save(new DB_visited_starsystems(starsystem.Info.id, add.ID, game.ID));
            #endregion


            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion
    }
}