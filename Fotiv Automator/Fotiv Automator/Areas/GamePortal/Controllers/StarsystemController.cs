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
    public class StarsystemController : DataController
    {
        [HttpGet]
        public override ActionResult Show(int? starsystemID)
        {
            Debug.WriteLine($"GET: Star Controller: View - starsystemID={starsystemID}");

            DB_users user = Auth.User;
            Game game = GameState.Game;

            return View(new ViewStarSystem
            {
                User = game.Players.Where(x => x.User.ID == user.id).First(),
                System = game.Sector.StarsystemFromID(starsystemID.Value)
            });
        }

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? starsystemID)
        {
            Debug.WriteLine($"GET: Star Controller: Edit - starsystemID={starsystemID}");

            var game = GameState.Game;
            var starsystem = game.Sector.StarsystemFromID(starsystemID.Value);

            return View(new StarsystemForm
            {
                ID = starsystem.ID,
                GMNotes = starsystem.Info.gmnotes
            });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public ActionResult Edit(StarsystemForm form, int? starsystemID)
        {
            Debug.WriteLine($"POST: Star Controller: Edit - starsystemID={starsystemID}");
            var game = GameState.Game;

            var starsystem = game.Sector.StarsystemFromID(starsystemID.Value);
            if (starsystem.Info.game_id != game.Info.id)
                return RedirectToRoute("game", new { gameID = game.Info.id });

            starsystem.Info.gmnotes = form.GMNotes;
            Database.Session.Update(starsystem.Info);

            Database.Session.Flush();
            return RedirectToRoute("game", new { gameID = game.Info.id });
        }
        #endregion
    }
}