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
    public class ResearchController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int researchID = -1)
        { 
            Debug.WriteLine(string.Format("GET: Research Controller: Index - researchID={0}", researchID));

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();
            if (game == null)
                return RedirectToRoute("home");

            return View(new IndexResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = game.GameStatistics.Research
            });
        }

        [HttpGet]
        public override ActionResult View(int researchID = -1)
        {
            if (researchID == -1) return RedirectToRoute("home");
            Debug.WriteLine(string.Format("GET: Research Controller: View - researchID={0}", researchID));

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewResearch
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Research = game.GameStatistics.Research.Find(x => x.id == researchID),
            });
        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {
            Debug.WriteLine(string.Format("GET: Research Controller: New"));
            return View(new ResearchForm());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New(ResearchForm form)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: New - gameID={0}", GameState.GameID));
            if (GameState.GameID == null) return RedirectToRoute("home");

            var game = GameState.Game;

            DB_research research = new DB_research();
            shipRate.name = form.Name;
            shipRate.build_rate = form.BuildRate;
            Database.Session.Save(research);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int researchID)
        {
            Debug.WriteLine(string.Format("GET: Research Controller: Edit - researchID={0}", researchID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var shipRate = game.GameStatistics.ShipRatesRaw.Find(x => x.id == researchID);
            return View(new ShipRateForm
            {
                ID = shipRate.id,
                Name = shipRate.name,
                BuildRate = shipRate.build_rate
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(ShipRateForm form, int researchID)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Edit - researchID={0}", researchID));

            var game = GameState.Game;
            if (game == null) return RedirectToRoute("home");

            var shipRate = game.GameStatistics.ShipRatesRaw.Find(x => x.id == researchID);
            shipRate.name = form.Name;
            shipRate.build_rate = form.BuildRate;
            Database.Session.Update(shipRate);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int researchID)
        {
            Debug.WriteLine(string.Format("POST: Research Controller: Delete - researchID={0}", researchID));

            var shipRate = Database.Session.Load<DB_ship_rates>(researchID);
            if (shipRate == null)
                return HttpNotFound();

            Database.Session.Delete(shipRate);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}