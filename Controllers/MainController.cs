using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Templater.Controllers
{
    //TODO: Переименовать в шаблонизатор
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Learn()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            return View();
        }
    }
}
