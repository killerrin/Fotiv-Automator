using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            Debug.WriteLine(string.Format("GET: Errors Controller: NotFound"));

            return View();
        }

        public ActionResult Error()
        {
            Debug.WriteLine(string.Format("GET: Errors Controller: Error"));

            return View();
        }
    }
}