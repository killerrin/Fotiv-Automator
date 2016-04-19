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
    public class UnitCategoryController : DataController
    {
        [HttpGet]
        public override ActionResult Index(int? gameID = null)
        { 
            Debug.WriteLine($"GET: Unit Category Controller: Index - gameID={gameID}");

            DB_users user = Auth.User;
            Game game = GameState.QueryGame();

            return View(new IndexUnitCategory
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                UnitCategories = game.GameStatistics.UnitCategoriesRaw
            });
        }

        [HttpGet]
        public override ActionResult Show(int? unitCategoryID)
        {
            Debug.WriteLine(string.Format("GET: Unit Category Controller: View - unitCategoryID={0}", unitCategoryID));
            if (unitCategoryID == -1)
                return RedirectToRoute("Statistics");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewUnitCategory
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                UnitCategory = game.GameStatistics.UnitCategoriesRaw.Find(x => x.id == unitCategoryID),
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            Debug.WriteLine(string.Format("GET: Unit Category Controller: New"));
            return View(new UnitCategoryForm());
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(UnitCategoryForm form)
        {
            Debug.WriteLine(string.Format("POST: Unit Category Controller: New - gameID={0}", GameState.GameID));
            var game = GameState.Game;

            DB_unit_categories category = new DB_unit_categories();
            category.game_id = game.Info.id;
            category.name = form.Name;
            category.build_rate = form.BuildRate;
            category.is_military = form.IsMilitary;
            Database.Session.Save(category);
            
            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? unitCategoryID)
        {
            Debug.WriteLine(string.Format("GET: Unit Category Controller: Edit - unitCategoryID={0}", unitCategoryID));
            var game = GameState.Game;

            var category = game.GameStatistics.UnitCategoriesRaw.Find(x => x.id == unitCategoryID);
            return View(new UnitCategoryForm
            {
                ID = category.id,
                Name = category.name,
                BuildRate = category.build_rate,
                IsMilitary = category.is_military
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(UnitCategoryForm form, int? unitCategoryID)
        {
            Debug.WriteLine(string.Format("POST: Unit Category Controller: Edit - unitCategoryID={0}", unitCategoryID));
            var game = GameState.Game;

            var category = game.GameStatistics.UnitCategoriesRaw.Find(x => x.id == unitCategoryID);
            if (category.game_id == null || category.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            category.name = form.Name;
            category.build_rate = form.BuildRate;
            category.is_military = form.IsMilitary;
            Database.Session.Update(category);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? unitCategoryID)
        {
            Debug.WriteLine(string.Format("POST: Unit Category Controller: Delete - unitCategoryID={0}", unitCategoryID));

            var category = Database.Session.Load<DB_unit_categories>(unitCategoryID);
            if (category == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (category.game_id == null || category.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(category);

            Database.Session.Flush();
            return RedirectToRoute("Statistics");
        }
    }
}