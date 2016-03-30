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
    public class CivilizationTraitController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int civilizationTraitID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: Index - civilizationTraitID={0}", civilizationTraitID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexCivilizationTrait
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                CivilizationTraits = game.GameStatistics.CivilizationTraits
            });
        }

        [HttpGet]
        public override ActionResult View(int civilizationTraitID = -1)
        {
            if (civilizationTraitID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: View - civilizationTraitID={0}", civilizationTraitID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewCivilizationTrait
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                CivilizationTrait = game.GameStatistics.CivilizationTraits.Find(x => x.id == civilizationTraitID),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: New"));
            return View(new CivilizationTraitForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(CivilizationTraitForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_civilization_traits civilizationTrait = new DB_civilization_traits();
            civilizationTrait.game_id = game.Info.id;
            civilizationTrait.name = form.Name;
            civilizationTrait.description = form.Description;
            civilizationTrait.local_influence_bonus = form.LocalInfluenceBonus;
            civilizationTrait.foreign_influence_bonus = form.ForeignInfluenceBonus;
            civilizationTrait.trade_bonus = form.TradeBonus;
            civilizationTrait.apply_military = form.ApplyMilitary;
            civilizationTrait.apply_units = form.ApplyUnits;
            civilizationTrait.apply_ships = form.ApplyShips;
            civilizationTrait.apply_infrastructure = form.ApplyInfrastructure;
            civilizationTrait.science_bonus = form.ScienceBonus;
            civilizationTrait.colonial_development_bonus = form.ColonialDevelopmentBonus;
            civilizationTrait.ship_construction_bonus = form.ShipConstructionBonus;
            civilizationTrait.unit_training_bonus = form.UnitTrainingBonus;
            Database.Session.Save(civilizationTrait);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int civilizationTraitID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: Edit - civilizationTraitID={0}", civilizationTraitID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilizationTrait = game.GameStatistics.CivilizationTraits.Find(x => x.id == civilizationTraitID);
            return View(new CivilizationTraitForm
            {
                ID = civilizationTrait.id,
                Name = civilizationTrait.name,
                Description = civilizationTrait.description,

                LocalInfluenceBonus = civilizationTrait.local_influence_bonus,
                ForeignInfluenceBonus = civilizationTrait.foreign_influence_bonus,
                TradeBonus = civilizationTrait.trade_bonus,

                ApplyMilitary = civilizationTrait.apply_military,
                ApplyUnits = civilizationTrait.apply_units,
                ApplyShips = civilizationTrait.apply_ships,
                ApplyInfrastructure = civilizationTrait.apply_infrastructure,

                ScienceBonus = civilizationTrait.science_bonus,
                ColonialDevelopmentBonus = civilizationTrait.colonial_development_bonus,
                ShipConstructionBonus = civilizationTrait.ship_construction_bonus,
                UnitTrainingBonus = civilizationTrait.unit_training_bonus
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(CivilizationTraitForm form, int civilizationTraitID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: Edit - civilizationTraitID={0}", civilizationTraitID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var civilizationTrait = game.GameStatistics.CivilizationTraits.Find(x => x.id == civilizationTraitID);
            if ((civilizationTrait.game_id == null || civilizationTrait.game_id != game.Info.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

            civilizationTrait.name = form.Name;
            civilizationTrait.description = form.Description;
            civilizationTrait.local_influence_bonus = form.LocalInfluenceBonus;
            civilizationTrait.foreign_influence_bonus = form.ForeignInfluenceBonus;
            civilizationTrait.trade_bonus = form.TradeBonus;
            civilizationTrait.apply_military = form.ApplyMilitary;
            civilizationTrait.apply_units = form.ApplyUnits;
            civilizationTrait.apply_ships = form.ApplyShips;
            civilizationTrait.apply_infrastructure = form.ApplyInfrastructure;
            civilizationTrait.science_bonus = form.ScienceBonus;
            civilizationTrait.colonial_development_bonus = form.ColonialDevelopmentBonus;
            civilizationTrait.ship_construction_bonus = form.ShipConstructionBonus;
            civilizationTrait.unit_training_bonus = form.UnitTrainingBonus;
            Database.Session.Update(civilizationTrait);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int civilizationTraitID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: Delete - civilizationTraitID={0}", civilizationTraitID));

            var civilizationTrait = Database.Session.Load<DB_civilization_traits>(civilizationTraitID);
            if (civilizationTrait == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if ((civilizationTrait.game_id == null || civilizationTrait.game_id != game.Info.id) && !User.IsInRole("Admin"))
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(civilizationTrait);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}