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
    public class InfrastructureController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int infrastructureID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Index - infrastructureID={0}", infrastructureID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = game.GameStatistics.Infrastructure
            });
        }

        [HttpGet]
        public override ActionResult View(int infrastructureID = -1)
        {
            if (infrastructureID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: View - infrastructureID={0}", infrastructureID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewInfrastructure
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = game.GameStatistics.Infrastructure.Find(x => x.Infrastructure.id == infrastructureID),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: New"));
            return View(new InfrastructureForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(InfrastructureForm form)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_infrastructure infrastructure = new DB_infrastructure();
            infrastructure.name = form.Name;
            infrastructure.description = form.Description;
            infrastructure.rp_cost = form.RPCost;

            infrastructure.is_colony = form.IsColony;
            infrastructure.is_military = form.IsMilitary;

            infrastructure.base_health = form.BaseHealth;
            infrastructure.base_attack = form.BaseAttack;
            infrastructure.influence = form.Influence;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;

            infrastructure.research_slot = form.ResearchSlot;
            infrastructure.ship_construction_slot = form.ShipConstructionSlot;
            infrastructure.colonial_development_slot = form.ColonialDevelopmentSlot;

            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Save(infrastructure);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int infrastructureID)
        {
            Debug.WriteLine(string.Format("GET: Infrastructure Controller: Edit - infrastructureID={0}", infrastructureID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            DB_infrastructure infrastructure = game.GameStatistics.InfrastructureRaw.Find(x => x.id == infrastructureID);
            return View(new InfrastructureForm
            {
                ID = infrastructure.id,

                Name        = infrastructure.name,
                Description = infrastructure.description,
                RPCost      = infrastructure.rp_cost,

                IsColony    = infrastructure.is_colony,
                IsMilitary  = infrastructure.is_military,

                BaseHealth  = infrastructure.base_health,
                BaseAttack  = infrastructure.base_attack,
                Influence   = infrastructure.influence,

                RPBonus                     = infrastructure.rp_bonus,
                ScienceBonus                = infrastructure.science_bonus,
                ShipConstructionBonus       = infrastructure.ship_construction_bonus,
                ColonialDevelopmentBonus    = infrastructure.colonial_development_bonus,

                ResearchSlot            = infrastructure.research_slot,
                ShipConstructionSlot    = infrastructure.ship_construction_slot,
                ColonialDevelopmentSlot = infrastructure.colonial_development_slot,

                GMNotes = infrastructure.gmnotes
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureForm form, int infrastructureID)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Edit - infrastructureID={0}", infrastructureID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            DB_infrastructure infrastructure = game.GameStatistics.InfrastructureRaw.Find(x => x.id == infrastructureID);
            infrastructure.name = form.Name;
            infrastructure.description = form.Description;
            infrastructure.rp_cost = form.RPCost;

            infrastructure.is_colony = form.IsColony;
            infrastructure.is_military = form.IsMilitary;

            infrastructure.base_health = form.BaseHealth;
            infrastructure.base_attack = form.BaseAttack;
            infrastructure.influence = form.Influence;

            infrastructure.rp_bonus = form.RPBonus;
            infrastructure.science_bonus = form.ScienceBonus;
            infrastructure.ship_construction_bonus = form.ShipConstructionBonus;
            infrastructure.colonial_development_bonus = form.ColonialDevelopmentBonus;

            infrastructure.research_slot = form.ResearchSlot;
            infrastructure.ship_construction_slot = form.ShipConstructionSlot;
            infrastructure.colonial_development_slot = form.ColonialDevelopmentSlot;

            infrastructure.gmnotes = form.GMNotes;
            Database.Session.Update(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int infrastructureID)
        {
            Debug.WriteLine(string.Format("POST: Infrastructure Controller: Delete - infrastructureID={0}", infrastructureID));

            var infrastructure = Database.Session.Load<DB_infrastructure>(infrastructureID);
            if (infrastructure == null)
                return HttpNotFound();

            Database.Session.Delete(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}