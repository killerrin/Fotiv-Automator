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
    public class RnDColonialDevelopmentController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: Index - {nameof(civilizationID)}={civilizationID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (civilizationID == null) return RedirectToRoute("home");

            var civilization = game.GetCivilization(civilizationID.Value);

            return View(new IndexRnDColonialDevelopment
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = civilization.Assets.IncompleteInfrastructure,
                CivilizationID = civilization.Info.id,
                CivilizationName = civilization.Info.name
            });
        }

        [HttpGet]
        public override ActionResult Show(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: View - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;
            var infrastructure = FindRNDInfrastructure(rndColonialDevelopmentID);

            return View(new ViewRnDColonialDevelopment
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Infrastructure = infrastructure,
                PlayerOwnsCivilization = game.GetCivilization(infrastructure.CivilizationInfo.civilization_id).PlayerOwnsCivilization(user.id)
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? civilizationID = null)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: New");
            return View(new RnDColonialDevelopmentForm
            {
                CivilizationID = civilizationID,
                Infrastructure = GameState.Game.GameStatistics.Infrastructure.Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, false)).ToList()
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(RnDColonialDevelopmentForm form)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: New");
            DB_users user = Auth.User;
            var game = GameState.Game;
            if (!game.GetCivilization(form.CivilizationID.Value).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            //DB_civilization_research research = new DB_civilization_research();
            //research.build_percentage = form.BuildPercentage;
            //research.research_id = form.SelectedResearched.Value;
            //research.civilization_id = form.CivilizationID.Value;
            //Database.Session.Save(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"GET: R&D Colonial Development Controller: Edit - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");
            DB_users user = Auth.User;
            Game game = GameState.Game;

            var infrastructure = FindRNDInfrastructure(rndColonialDevelopmentID);
            var infrastructureCheckBoxes = GameState.Game.GameStatistics.Infrastructure.Select(x => new Checkbox(x.Infrastructure.id, x.Infrastructure.name, x.Infrastructure.id == infrastructure.CivilizationInfo.struct_id)).ToList();

            return View(new RnDColonialDevelopmentForm
            {
                ID = infrastructure.CivilizationInfo.id,
                CivilizationID = infrastructure.CivilizationInfo.civilization_id,
                BuildPercentage = infrastructure.CivilizationInfo.build_percentage,

                Infrastructure = infrastructureCheckBoxes,
                SelectedInfrastructureID = infrastructureCheckBoxes.Where(x => x.IsChecked).First().ID
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(RnDColonialDevelopmentForm form, int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: Edit");
            DB_users user = Auth.User;
            var game = GameState.Game;

            //DB_civilization_research research = FindRNDInfrastructure(rndColonialDevelopmentID).CivilizationInfo;
            //if (!RequireGMAdminAttribute.IsGMOrAdmin())
            //    return RedirectToRoute("game", new { gameID = game.Info.id });
            //
            //research.build_percentage = form.BuildPercentage;
            //research.research_id = form.SelectedResearched.Value;
            //research.civilization_id = form.CivilizationID.Value;
            //Database.Session.Update(research);

            Database.Session.Flush();
            return RedirectToRoute("ViewCivilization", new { civilizationID = form.CivilizationID.Value });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? rndColonialDevelopmentID)
        {
            Debug.WriteLine($"POST: R&D Colonial Development Controller: Delete - {nameof(rndColonialDevelopmentID)}={rndColonialDevelopmentID}");

            var infrastructure = Database.Session.Load<DB_civilization_infrastructure>(rndColonialDevelopmentID);
            if (infrastructure == null)
                return HttpNotFound();

            DB_users user = Auth.User;
            Game game = GameState.Game;
            if (!game.GetCivilization(infrastructure.civilization_id).PlayerOwnsCivilization(user.id) && !RequireGMAdminAttribute.IsGMOrAdmin())
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(infrastructure);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }

        #region Tools
        private Models.Game.Infrastructure FindRNDInfrastructure(int? rndColonialDevelopmentID)
        {
            Game game = GameState.Game;

            foreach (var civilization in game.Civilizations)
                foreach (var rnd in civilization.Assets.InfrastructureRaw)
                    if (rnd?.CivilizationInfo.id == rndColonialDevelopmentID)
                        return rnd;

            return null;
        }
        #endregion
    }
}