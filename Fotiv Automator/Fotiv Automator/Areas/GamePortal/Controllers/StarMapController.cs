using Fotiv_Automator.Infrastructure.Attributes;
using Fotiv_Automator.Infrastructure.CustomControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    [RequireGame]
    public class StarMapController : NewViewEditDeleteController
    {
        // GET: GamePortal/StarMap
        [HttpGet]
        public override ActionResult Index(int? id = null)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public override ActionResult View(int? id)
        {
            throw new NotImplementedException();
        }

        #region New
        [HttpGet, RequireGMAdmin]
        public override ActionResult New(int? id = null)
        {
            throw new NotImplementedException();
        }

        //[HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        //public abstract ActionResult New(object objForm);
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public override ActionResult Edit(int? id)
        {
            throw new NotImplementedException();
        }

        //[HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        //public abstract ActionResult Edit(object objForm, int id);
        #endregion

        [HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        public override ActionResult Delete(int? id)
        {
            throw new NotImplementedException();
        }
    }
}