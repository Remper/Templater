using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Templater.Models;
using System.Web.Security;
using System.Data;

namespace Templater.Controllers
{
    /// <summary>
    /// Контроллер для управления авторизацией пользователя
    /// </summary>
    public class AuthController : Controller
    {
        //
        // GET: /Auth/

        [HttpGet]
        [ActionName("Index")]
        public ActionResult LogOn()
        {
            User curUser = new User(Request["Email"]);
            if (curUser.Authorize(Request["Password"]))
            {
                FormsAuthentication.SetAuthCookie(curUser.Email, true);
                Session["User"] = curUser;
            }
            return Json(curUser, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Json(new {result = true}, JsonRequestBehavior.AllowGet);
        }
    }
}
