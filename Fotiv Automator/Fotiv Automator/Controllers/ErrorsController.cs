﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fotiv_Automator.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}