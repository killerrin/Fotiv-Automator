using Fotiv_Automator.Infrastructure.CustomControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Areas.GamePortal.Controllers
{
    public class RnDColonialDevelopmentController : NewViewEditDeleteController
    {
        [HttpGet]
        public override ActionResult Index(int id = -1)
        {

        }

        [HttpGet]
        public override ActionResult View(int id)
        {

        }

        #region New
        [HttpGet]
        public override ActionResult New()
        {

        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult New(object objForm)
        //{
        //
        //}
        #endregion

        #region Edit
        [HttpGet]
        public override ActionResult Edit(int id)
        {

        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(object objForm, int id)
        //{
        //
        //}
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Delete(int id)
        {

        }
    }
}