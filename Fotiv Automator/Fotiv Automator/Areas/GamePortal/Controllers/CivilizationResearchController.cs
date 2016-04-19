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
    public class CivilizationResearchController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? civilizationResearchID)
        {
            Debug.WriteLine(string.Format("GET: Civilization Research Controller: View - civilizationResearchID={0}", civilizationResearchID));

            DB_users user = Auth.User;
            Game game = GameState.Game;
            Research research = FindCivilizationResearch(civilizationResearchID);

            return View(new ViewCivilizationResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = research,
                PlayerOwnsCivilization = game.GetCivilization(research.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine(string.Format("GET: Civilization Research Controller: New"));
            return View(new CivilizationResearchForm
            {
                CivilizationID = civilizationID,
                Research = GameState.Game.GameStatistics.Research.Select(x => new Checkbox(x.id, x.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(CivilizationResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: Civilization Research Controller: New"));
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_research research = new DB_civilization_research();
            research.game_id = game.ID;
            research.research_id = form.SelectedResearchID.Value;
            research.civilization_id = form.CivilizationID.Value;
            Database.Session.Save(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? civilizationResearchID)
        {
            Debug.WriteLine($"GET: Civilization Research Controller: Edit - {nameof(civilizationResearchID)}={civilizationResearchID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            Research research = FindCivilizationResearch(civilizationResearchID);
            var researchCheckBoxes = GameState.Game.GameStatistics.Research.Select(x => new Checkbox(x.id, x.name, x.id == research.CivilizationInfo.research_id)).ToList();

            return View(new CivilizationResearchForm
            {
                ID = research.CivilizationInfo.id,
                CivilizationID = research.CivilizationInfo.civilization_id,

                Research = researchCheckBoxes,
                SelectedResearchID = researchCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(CivilizationResearchForm form, int? civilizationResearchID)
        {
            Debug.WriteLine($"POST: Civilization Research Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_research research = FindCivilizationResearch(civilizationResearchID).CivilizationInfo;
            if (research.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                research.research_id = form.SelectedResearchID.Value;
                research.civilization_id = form.CivilizationID.Value;
            }
            Database.Session.Update(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? civilizationResearchID)
        {
            Debug.WriteLine(string.Format("POST: R&D Research Controller: Delete - civilizationResearchID={0}", civilizationResearchID));

            var research = Database.Session.Load<DB_civilization_research>(civilizationResearchID);
            if (research == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(research.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                research.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            int tmpCivilizationID = research.civilization_id;
            Database.Session.Delete(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = tmpCivilizationID });
        }

        #region Tools
        private Research FindCivilizationResearch(int? civilizationResearchID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.CompletedResearch)
                    if (rnd?.CivilizationInfo.id == civilizationResearchID)
                        return rnd;

            return null;
        }
        #endregion
    }
}