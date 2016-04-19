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
    public class CivilizationRnDResearchController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? rndResearchID)
        {
            Debug.WriteLine($"GET: Civilization RnD Research Controller: View - {nameof(rndResearchID)}={rndResearchID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var research = FindRNDCivilizationResearch(rndResearchID);

            return View(new ViewCivilizationRnDResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = research,
                PlayerOwnsCivilization = game.GetCivilization(research.Info.civilization_id).PlayerOwnsCivilization(user.id),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Research Controller: New");
            Game game = GameState.Game;
            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new RnDResearchForm
            {
                CivilizationID = civilizationID,
                Research = game.GameStatistics.Research.Select(x => new Checkbox(x.id, x.name, false)).ToList(),
                BuildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.research_slot == true)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", false))
                    .ToList(),
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDResearchForm form)
        {
            Debug.WriteLine($"POST: Civilization RnD Research Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;

            var dbResearch = game.GameStatistics.Research
                .Where(x => x.id == form.SelectedResearchID.Value)
                .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (dbResearch == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            DB_civilization_rnd_research research = new DB_civilization_rnd_research();
            research.game_id = game.ID;
            research.civilization_id = form.CivilizationID.Value;
            research.research_id = dbResearch.id;
            research.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
            research.build_percentage = form.BuildPercentage;
            Database.Session.Save(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? rndResearchID)
        {
            Debug.WriteLine($"GET: Civilization RND Research Controller: Edit - {nameof(rndResearchID)}={rndResearchID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var research = FindRNDCivilizationResearch(rndResearchID);
            var researchCheckBoxes = game.GameStatistics.Research
                .Select(x => new Checkbox(x.id, x.name, x.id == research.Info.research_id))
                .ToList();

            var civilization = game.GetCivilization(research.Info.civilization_id);
            var buildAtInfrastructure = civilization.Assets.CompletedInfrastructure
                    .Where(x => x.InfrastructureInfo.Infrastructure.research_slot == true)
                    .Select(x => new Checkbox(x.CivilizationInfo.id, $"{x.CivilizationInfo.name} - {x.InfrastructureInfo.Infrastructure.name}", x.CivilizationInfo.id == research.Info.civ_struct_id))
                    .ToList();

            return View(new RnDResearchForm
            {
                ID = research.Info.id,
                CivilizationID = research.Info.civilization_id,

                BuildPercentage = research.Info.build_percentage,

                Research = researchCheckBoxes,
                SelectedResearchID = researchCheckBoxes.Where(x => x.IsChecked).First().ID,

                BuildAtInfrastructure = buildAtInfrastructure,
                SelectedBuildAtInfrastructureID = buildAtInfrastructure.Where(x => x.IsChecked).First().ID,           
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(RnDResearchForm form, int? rndResearchID)
        {
            Debug.WriteLine($"POST: Civilization RND Research Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            DB_civilization_rnd_research research = FindRNDCivilizationResearch(rndResearchID).Info;
            if (research.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            var dbResearch = game.GameStatistics.Research
                .Where(x => x.id == form.SelectedResearchID.Value)
                .FirstOrDefault();
            var dbBuildAtStruct = game.GetCivilization(form.CivilizationID.Value).Assets.CompletedInfrastructure
                .Where(x => x.CivilizationInfo.id == form.SelectedBuildAtInfrastructureID)
                .FirstOrDefault();

            if (dbResearch == null || dbBuildAtStruct == null)
                return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });

            if (RequireGMAdminAttribute.IsGMOrAdmin())
            {
                research.civilization_id = form.CivilizationID.Value;
                research.research_id = dbResearch.id;
                research.civ_struct_id = dbBuildAtStruct.CivilizationInfo.id;
            }
            Database.Session.Update(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndResearchID)
        {
            Debug.WriteLine($"POST: Civilization RND Research Controller: Delete - {nameof(rndResearchID)}={rndResearchID}");

            var research = Database.Session.Load<DB_civilization_rnd_research>(rndResearchID);
            if (research == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if ((!game.GetCivilization(research.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin()) ||
                research.game_id != game.ID)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(research);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private RnDResearch FindRNDCivilizationResearch(int? rndResearchID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.IncompleteResearch)
                    if (rnd?.Info.id == rndResearchID)
                        return rnd;

            return null;
        }
        #endregion
    }
}