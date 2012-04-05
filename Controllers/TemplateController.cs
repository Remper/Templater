using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templater.Models;

namespace Templater.Controllers
{
    /// <summary>
    /// Контроллер для обработки данных связанных с шаблонами
    /// </summary>
    public class TemplateController : Controller
    {
        //
        // GET: /Template/

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult New()
        {
            int ownerId = ((User)Session["User"]).UserId;
            string name = Request["name"];
            string website = Request["website"];
            string templateData = Request["source"];

            //Создаём шаблон
            Object response;
            try
            {
                if (!Template.NameReg.IsMatch(name) || !Template.WebsiteReg.IsMatch(website) || !Template.TemplateDataReg.IsMatch(templateData))
                    throw new Exception("Regexp doesn't match");

                Template result = Template.CreateTemplate(ownerId, name, website, templateData == "null" ? null : templateData);
                response = new { result = true };
            }
            catch (Exception e)
            {
                response = new { result = false, msg = e.Message };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit()
        {
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Learn()
        {
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Delete()
        {
            //Допилить парсинг инта + уметь узнавать принадлежит ли пользователю шаблон + запилить клиентскую часть
            Template.DeleteTemplate(0, ((User)Session["User"]).UserId);
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

    }
}
