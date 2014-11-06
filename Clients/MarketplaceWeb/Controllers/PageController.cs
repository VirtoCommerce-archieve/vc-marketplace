﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketplaceWeb.Controllers
{
    using System.Web.Mvc;

    [RoutePrefix("pages")]
    public class PageController : Controller
    {
        [Route("{pagename}")]
        public ActionResult DisplayPage(string pageName)
        {
            return View(pageName);
        }
    }
}