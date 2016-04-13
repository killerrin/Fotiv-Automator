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
            var game = GameState.Game;

            var starAges = new List<Checkbox>();
            starAges.Add(new Checkbox(-1, "None", true));
            foreach (var age in game.GameStatistics.StarAges)
                starAges.Add(new Checkbox(age.id, age.name, false));

            var starTypes = new List<Checkbox>();
            starTypes.Add(new Checkbox(-1, "None", true));
            foreach (var type in game.GameStatistics.StarTypes)
                starTypes.Add(new Checkbox(type.id, type.name, false));

            var radiationLevels = new List<Checkbox>();
            radiationLevels.Add(new Checkbox(-1, "None", true));
            foreach (var radiationLevel in game.GameStatistics.RadiationLevels)
                radiationLevels.Add(new Checkbox(radiationLevel.id, radiationLevel.name, false));

            return View(new StarForm
            {
                StarsystemID = starsystemID.Value,

                StarAges = starAges,
                StarTypes = starTypes,
                RadiationLevels = radiationLevels
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

            star.star_age_id = (form.SelectedStarAge == -1) ? null : form.SelectedStarAge;
            star.star_type_id = (form.SelectedStarType == -1) ? null : form.SelectedStarType;
            star.radiation_level_id = (form.SelectedRadiationLevel == -1) ? null : form.SelectedRadiationLevel;

            star.name = form.Name;
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

            var starAges = new List<Checkbox>();
            starAges.Add(new Checkbox(-1, "None", false));
            foreach (var age in game.GameStatistics.StarAges)
                starAges.Add(new Checkbox(age.id, age.name, star.star_age_id == age.id));
            if (starAges.Where(x => x.IsChecked).ToList().Count == 0) starAges[0].IsChecked = true;

            var starTypes = new List<Checkbox>();
            starTypes.Add(new Checkbox(-1, "None", false));
            foreach (var type in game.GameStatistics.StarTypes)
                starTypes.Add(new Checkbox(type.id, type.name, star.star_type_id == type.id));
            if (starTypes.Where(x => x.IsChecked).ToList().Count == 0) starTypes[0].IsChecked = true;

            var radiationLevels = new List<Checkbox>();
            radiationLevels.Add(new Checkbox(-1, "None", false));
            foreach (var radiationLevel in game.GameStatistics.RadiationLevels)
                radiationLevels.Add(new Checkbox(radiationLevel.id, radiationLevel.name, star.radiation_level_id == radiationLevel.id));
            if (radiationLevels.Where(x => x.IsChecked).ToList().Count == 0) radiationLevels[0].IsChecked = true;

            return View(new StarForm
            {
                ID = star.id,
                StarsystemID = star.starsystem_id,
                Name = star.name,
                GMNotes = star.gmnotes,

                StarAges = starAges,
                StarTypes = starTypes,
                RadiationLevels = radiationLevels
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

            star.star_age_id = (form.SelectedStarAge == -1) ? null : form.SelectedStarAge;
            star.star_type_id = (form.SelectedStarType == -1) ? null : form.SelectedStarType;
            star.radiation_level_id = (form.SelectedRadiationLevel == -1) ? null : form.SelectedRadiationLevel;

            star.name = form.Name;
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