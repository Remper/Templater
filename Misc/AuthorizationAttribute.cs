using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcFilters.Infrastructure.Filters
{
    public class AuthorizationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Session["User"] == null)
            {
                FormsAuthentication.SignOut();
                context.HttpContext.Session.Abandon();
                context.HttpContext.Response.Redirect("/");
            }
        }
    }
}