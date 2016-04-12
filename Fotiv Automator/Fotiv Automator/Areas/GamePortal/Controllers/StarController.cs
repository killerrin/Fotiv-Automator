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
    public class StarController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? starID)
        {
            Debug.WriteLine($"GET: Star Controller: View - starID={starID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewStar
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                Star = game.Sector.StarFromID(starID.Value)
            });
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? starsystemID = null)
        {
            Debug.WriteLine($"GET: Star Controller: New - starsystemID={starsystemID}");
            return View(new StarForm
            {
                StarsystemID = starsystemID.Value
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult New(StarForm form, int? starsystemID = null)
        {
            Debug.WriteLine($"POST: Star Controller: New - starsystemID={form.StarsystemID}");
            var game = GameState.Game;

            DB_stars star = new DB_stars();
            star.game_id = game.ID;
            star.starsystem_id = form.StarsystemID;
            star.name = form.Name;
            star.age = form.Age;
            star.radiation_level = form.RadiationLevel;
            star.gmnotes = form.GMNotes;
            Database.Session.Save(star);
            
            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? starID)
        {
            Debug.WriteLine($"GET: Star Controller: Edit - starID={starID}");

            var game = GameState.Game;
            var star = game.Sector.StarFromID(starID.Value).Info;

            return View(new StarForm
            {
                ID = star.id,
                StarsystemID = star.starsystem_id,
                Name = star.name,
                Age = star.age,
                RadiationLevel = star.radiation_level,
                GMNotes = star.gmnotes
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StarForm form, int? starID)
        {
            Debug.WriteLine($"POST: Star Controller: Edit - starID={starID}");
            var game = GameState.Game;

            var star = game.Sector.StarFromID(starID.Value).Info;
            if (star.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            star.name = form.Name;
            star.age = form.Age;
            star.radiation_level = form.RadiationLevel;
            Database.Session.Update(star);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? starID)
        {
            Debug.WriteLine($"POST: Star Controller: Delete - starID={starID}");

            var star = Database.Session.Load<DB_stars>(starID.Value);
            if (star == null)
                return HttpNotFound();

            Game game = GameState.Game;
            if (star.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            Database.Session.Delete(star);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = GameState.GameID });
        }
    }
}