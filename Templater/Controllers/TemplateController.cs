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
            int templateID;
            bool result;
            if (!Int32.TryParse(Request["templateid"], out templateID) || templateID <= 0)
                return Json(new { result = false, msg = "Не могу распарсить входной параметр" }, JsonRequestBehavior.AllowGet);

            if (((User)Session["User"]).CheckRightsForTemplate(templateID))
                result = Template.DeleteTemplate(templateID);
            else
                return Json(new { result = false, msg = "Это не ваш шаблон" }, JsonRequestBehavior.AllowGet);

            if (result)
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { result = false, msg = "Произошла ошибка во время удаления шаблона" }, JsonRequestBehavior.AllowGet);
        }

    }
}
