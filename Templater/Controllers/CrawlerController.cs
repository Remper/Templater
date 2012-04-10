using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templater.Controllers
{
    public class CrawlerController : Controller
    {
        //
        // GET: /Crawler/

        public ActionResult Index()
        {
            ViewBag.Controller = "crawler";
            return View();
        }

    }
}
