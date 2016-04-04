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
    public class SpeciesController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int? speciesID = null)
        { 
            Debug.WriteLine(string.Format("GET: Species Controller: Index - speciesID={0}", speciesID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexSpecies
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Species = game.GameStatistics.Species
            });
        }

        [HttpGet]
        public override ActionResult View(int? speciesID)
        {
            Debug.WriteLine(string.Format("GET: Species Controller: View - speciesID={0}", speciesID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewSpecies
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Species = game.GameStatistics.Species.Find(x => x.id == speciesID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Species Controller: New"));
            var game = GameState.Game;
            return View(new SpeciesForm
            {
                Civilizations = game.Civilizations.Select(x => new Checkbox(x.Info.id, x.Info.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(SpeciesForm form)
        {
            Debug.WriteLine(string.Format("POST: Species Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_species species = new DB_species();
            species.game_id = game.Info.id;
            species.name = form.Name;
            species.description = form.Description;
            species.base_attack = form.BaseAttack;
            species.base_special_attack = form.BaseSpecialAttack;
            species.base_health = form.BaseHealth;
            species.base_regeneration = form.BaseRegeneration;
            species.base_agility = form.BaseAgility;
            species.gmnotes = form.GMNotes;
            Database.Session.Save(species);

            var checkedCivilizations = form.Civilizations
                .Where(x => x.IsChecked)
                .ToList();

            foreach (var civilizations in checkedCivilizations)
            {
                DB_civilization_species civilizationSpecies = new DB_civilization_species();
                civilizationSpecies.civilization_id = civilizations.ID;
                civilizationSpecies.species_id = species.id;
                Database.Session.Save(civilizationSpecies);
            }

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? speciesID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Edit - speciesID={0}", speciesID));
            var game = GameState.Game;

            DB_species species = game.GameStatistics.Species.Find(x => x.id == speciesID);
            var civilizations = game.Civilizations.Select(x => new Checkbox(x.Info.id, x.Info.name, x.CivilizationHasSpecies(species.id))).ToList();
            return View(new SpeciesForm
            {
                ID = species.id,

                Name = species.name,
                Description = species.description,

                BaseAttack = species.base_attack,
                BaseSpecialAttack = species.base_special_attack,
                BaseHealth = species.base_health,
                BaseRegeneration = species.base_regeneration,
                BaseAgility = species.base_agility,

                GMNotes = species.gmnotes,

                Civilizations = civilizations
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(SpeciesForm form, int? speciesID)
        {
            Debug.WriteLine(string.Format("POST: Species Controller: Edit - speciesID={0}", speciesID));
            var game = GameState.Game;

            DB_species species = game.GameStatistics.Species.Find(x => x.id == speciesID);
            if (species.game_id == null || species.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            species.name = form.Name;
            species.description = form.Description;
            species.base_attack = form.BaseAttack;
            species.base_special_attack = form.BaseSpecialAttack;
            species.base_health = form.BaseHealth;
            species.base_regeneration = form.BaseRegeneration;
            species.base_agility = form.BaseAgility;
            species.gmnotes = form.GMNotes;
            Database.Session.Update(species);

            var civilizationSpecies = Database.Session.Query<DB_civilization_species>()
                .Where(x => x.species_id == species.id)
                .ToList();

            var checkedCivilizations = form.Civilizations
                .Where(x => x.IsChecked)
                .ToList();

            List<DB_civilization_species> toRemove = new List<DB_civilization_species>();
            List<Checkbox> toAdd = new List<Checkbox>();

            foreach (var civSpecie in civilizationSpecies)
            {
                bool foundMatch = false;

                // First determine what to remove
                foreach (var checkBox in checkedCivilizations)
                {
                    // Civilization is already set
                    if (civSpecie.civilization_id == checkBox.ID)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // No longer an Upgrade to this Infrastructure
                if (!foundMatch)
                    toRemove.Add(civSpecie);
            }

            // Next determine what is new
            foreach (var checkBox in checkedCivilizations)
            {
                bool foundMatch = false;
                foreach (var civSpecie in civilizationSpecies)
                {
                    // Infrastructure Upgrade is already set
                    if (checkBox.ID == civSpecie.civilization_id)
                    {
                        foundMatch = true;
                        break;
                    }
                }

                // We have a new upgrade
                if (!foundMatch)
                    toAdd.Add(checkBox);
            }

            // Now apply the changes
            foreach (var remove in toRemove)
                Database.Session.Delete(remove);
            foreach (var add in toAdd)
                Database.Session.Save(new DB_civilization_species(species.id, add.ID, game.Info.id));

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? speciesID)
        {
            Debug.WriteLine(string.Format("POST: Species Controller: Delete - speciesID={0}", speciesID));

            var species = Database.Session.Load<DB_species>(speciesID);
            if (species == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (species.game_id == null || species.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(species);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}