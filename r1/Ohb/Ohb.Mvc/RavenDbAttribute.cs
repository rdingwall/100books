using System.Web.Mvc;
using Ohb.Mvc.Api.Controllers;

namespace Ohb.Mvc
{
    public class RavenDbAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as OhbControllerBase;

            if (controller == null)
                return;

            var session = controller.DocumentSession;
            session.SaveChanges();
            session.Dispose();
        }
    }
}