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
        public abstract ActionResult Index(int id = -1);

        [HttpGet]
        public abstract ActionResult View(int id);

        #region New
        [HttpGet]
        public abstract ActionResult New();

        [HttpPost, ValidateAntiForgeryToken]
        public abstract ActionResult New(object form);
        #endregion

        #region Edit
        [HttpGet]
        public abstract ActionResult Edit(int id);

        [HttpPost, ValidateAntiForgeryToken]
        public abstract ActionResult Edit(object form, int id);
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public abstract ActionResult Delete(int id);
    }
}
