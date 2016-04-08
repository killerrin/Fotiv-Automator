using Fotiv_Automator.Areas.GamePortal.Models.Game;
using Fotiv_Automator.Areas.GamePortal.ViewModels;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Checkboxes;
using Fotiv_Automator.Areas.GamePortal.ViewModels.Forms;
using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Infrastructure.CustomControllers;
using Fotiv_Automator.Models.DatabaseMaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class RnDResearchController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationID = null)
        {
            Debug.WriteLine(string.Format("GET: R&D Research Controller: Index - civilizationID={0}", civilizationID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (civilizationID == null) return RedirectToRoute("home");

            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new IndexRnDResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = civilization.Assets.IncompleteResearch,
                CivilizationID = civilization.Info.id,
                CivilizationName = civilization.Info.name
            });
        }

        [HttpGet]
        public override ActionResult Show(int? rndResearchID)
        {
            Debug.WriteLine(string.Format("GET: R&D Research Controller: View - rndResearchID={0}", rndResearchID));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            Research research = FindRNDResearch(rndResearchID);

            return View(new ViewRnDResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = research,
                PlayerOwnsCivilization = game.GetCivilization(research.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine(string.Format("GET: R&D Research Controller: New"));
            return View(new RnDResearchForm
            {
                CivilizationID = civilizationID,
                Research = GameState.Game.GameStatistics.Research.Select(x => new Checkbox(x.id, x.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: R&D Research Controller: New"));
            DB_users user = Auth.User;
            var game = GameState.Game;
            if (!game.GetCivilization(form.CivilizationID.Value).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            DB_civilization_research research = new DB_civilization_research();
            research.game_id = game.ID;
            research.build_percentage = form.BuildPercentage;
            research.research_id = form.SelectedResearchID.Value;
            research.civilization_id = form.CivilizationID.Value;
            Database.Session.Save(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? rndResearchID)
        {
            Debug.WriteLine($"GET: R&D Research Controller: Edit - {nameof(rndResearchID)}={rndResearchID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            Research research = FindRNDResearch(rndResearchID);
            var researchCheckBoxes = GameState.Game.GameStatistics.Research.Select(x => new Checkbox(x.id, x.name, x.id == research.CivilizationInfo.research_id)).ToList();

            return View(new RnDResearchForm
            {
                ID = research.CivilizationInfo.id,
                CivilizationID = research.CivilizationInfo.civilization_id,
                BuildPercentage = research.CivilizationInfo.build_percentage,

                Research = researchCheckBoxes,
                SelectedResearchID = researchCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RnDResearchForm form, int? rndResearchID)
        {
            Debug.WriteLine($"POST: R&D Research Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_research research = FindRNDResearch(rndResearchID).CivilizationInfo;
            if (!RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });
            
            research.build_percentage = form.BuildPercentage;
            research.research_id = form.SelectedResearchID.Value;
            research.civilization_id = form.CivilizationID.Value;
            Database.Session.Update(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndResearchID)
        {
            Debug.WriteLine(string.Format("POST: R&D Research Controller: Delete - rndResearchID={0}", rndResearchID));

            var research = Database.Session.Load<DB_civilization_research>(rndResearchID);
            if (research == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if (!game.GetCivilization(research.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(research);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private Research FindRNDResearch(int? rndResearchID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.ResearchRaw)
                    if (rnd?.CivilizationInfo.id == rndResearchID)
                        return rnd;

            return null;
        }
        #endregion
    }
}