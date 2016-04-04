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
    public class RnDShipConstructionController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int? id = null)
        {
            return View();
        }

        [HttpGet]
        public override ActionResult View(int? id)
        {
            return View();
        }

        #region New
        [HttpGet]
        public override ActionResult New(int? id = null)
        {
            return View();
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult New(object objForm)
        //{
        //  return View();
        //}
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int? id)
        {
            return View();
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(object objForm, int? id)
        //{
        //  return View();
        //}
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int? id)
        {
            return View();
        }
    }
}