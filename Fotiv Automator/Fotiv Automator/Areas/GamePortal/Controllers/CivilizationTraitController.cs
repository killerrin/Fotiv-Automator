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
    public class CivilizationTraitController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationTraitID = null)
        { 
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: Index - civilizationTraitID={0}", civilizationTraitID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexCivilizationTraits
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                CivilizationTraits = game.GameStatistics.CivilizationTraits
            });
        }

        [HttpGet]
        public override ActionResult Show(int? civilizationTraitID)
        {
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
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: New"));
            return View(new CivilizationTraitForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(CivilizationTraitForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_civilization_traits civilizationTrait = new DB_civilization_traits();
            civilizationTrait.game_id = game.Info.id;
            civilizationTrait.name = form.Name;
            civilizationTrait.description = form.Description;
            civilizationTrait.domestic_influence_bonus = form.DomesticInfluenceBonus;
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
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? civilizationTraitID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Trait Controller: Edit - civilizationTraitID={0}", civilizationTraitID));
            var game = GameState.Game;

            var civilizationTrait = game.GameStatistics.CivilizationTraits.Find(x => x.id == civilizationTraitID);
            return View(new CivilizationTraitForm
            {
                ID = civilizationTrait.id,
                Name = civilizationTrait.name,
                Description = civilizationTrait.description,

                DomesticInfluenceBonus = civilizationTrait.domestic_influence_bonus,
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

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(CivilizationTraitForm form, int? civilizationTraitID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: Edit - civilizationTraitID={0}", civilizationTraitID));
            var game = GameState.Game;

            var civilizationTrait = game.GameStatistics.CivilizationTraits.Find(x => x.id == civilizationTraitID);
            if (civilizationTrait.game_id == null || civilizationTrait.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            civilizationTrait.name = form.Name;
            civilizationTrait.description = form.Description;
            civilizationTrait.domestic_influence_bonus = form.DomesticInfluenceBonus;
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
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? civilizationTraitID)
        {
            Debug.WriteLine(string.Format("POST: Civilization Trait Controller: Delete - civilizationTraitID={0}", civilizationTraitID));

            var civilizationTrait = Database.Session.Load<DB_civilization_traits>(civilizationTraitID);
            if (civilizationTrait == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (civilizationTrait.game_id == null || civilizationTrait.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(civilizationTrait);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}