using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Templater.Adapters;
using Templater.Models;

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
        public ActionResult List()
        {
            //Инициализируем базу данных
            MysqlDatabase database = new MysqlDatabase(WebConfigurationManager.AppSettings["ConnectionString"]);
            //Получаем список шаблонов для текущего пользователя
            Template[] templates = database.GetTemplates(((User)Session["User"]).UserId);

            //Отправляем модель на отрисовку
            return View(templates);
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
