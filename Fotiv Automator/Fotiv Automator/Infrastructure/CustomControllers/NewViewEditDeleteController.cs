using Fotiv_Automator.Infrastructure.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fotiv_Automator.Infrastructure.CustomControllers
{
    public abstract class NewViewEditDeleteController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index(int? id = null) { return View(); }

        [HttpGet]
        public virtual ActionResult View(int? id) { return View(); }

        #region New
        [HttpGet, RequireGMAdmin]
        public virtual ActionResult New(int? id = null) { return View(); }

        //[HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        //public virtual ActionResult New(object objForm)
        //{
        //   return View(); 
        //}
        #endregion

        #region Edit
        [HttpGet, RequireGMAdmin]
        public virtual ActionResult Edit(int? id) { return View(); }

        //[HttpPost, ValidateAntiForgeryToken, RequireGMAdmin]
        //public virtual ActionResult Edit(object objForm, int? id)
        //{
        //   return View(); 
        //}
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int? id) { return View(); }
    }
}
