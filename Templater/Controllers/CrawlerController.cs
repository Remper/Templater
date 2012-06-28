using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Templater.Models;
using Templater.Misc;
using System.Web.Security;

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

        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            //Получаем список задач для текущего пользователя
            Task[] tasks = Database.Instance.GetTasks(((User)Session["User"]).UserId);
            //Рендерим 
            String data = Render.RenderView(this, "List", tasks);

            //Отправляем ответ
            return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            //Рендерим 
            String data = Render.RenderView(this, "New", null);

            //Отправляем ответ
            return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}
