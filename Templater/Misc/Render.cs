using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;

namespace Templater.Misc
{
    public static class Render
    {
        public static string RenderPartialView(Controller controller, string viewName, object model)
        {
            return Render.RenderToString(controller, viewName, model, true);
        }

        public static string RenderView(Controller controller, string viewName, object model)
        {
            return Render.RenderToString(controller, viewName, model, false);
        }

        private static string RenderToString(Controller controller, string viewName, object model, bool isPartial)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult;
                    if (isPartial)
                        viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    else
                        viewResult = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, null);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}