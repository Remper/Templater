using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Templater.Adapters;
using Templater.Models;
using Templater.Misc;
using System.Web.Security;

namespace Templater.Controllers
{
    //TODO: Переименовать в шаблонизатор
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            ViewBag.Controller = "templater";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult List()
        {
            //Инициализируем базу данных
            MysqlDatabase database = new MysqlDatabase(WebConfigurationManager.AppSettings["ConnectionString"]);
            //Получаем список шаблонов для текущего пользователя
            Template[] templates = database.GetTemplates(((User)Session["User"]).UserId);
            //Рендерим 
            String data = Render.RenderView(this, "List", templates);

            //Отправляем ответ
            return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet); ;
        }

        [HttpGet]
        [Authorize]
        public ActionResult New()
        {
            //Рендерим 
            String data = Render.RenderView(this, "New", null);

            //Отправляем ответ
            return Json(new { result = true, data = data }, JsonRequestBehavior.AllowGet); ;
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
