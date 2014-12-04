using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketplaceWeb.Controllers
{
    [RoutePrefix("extension")]
    public class CatalogController : ControllerBase
    {
        // GET: Extensions
        [Route("list/{categoryId}")]
        public ActionResult Display(string categoryId)
        {

            return View("Category");
        }
    }
}