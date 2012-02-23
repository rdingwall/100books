using System.Web.Mvc;

namespace Ohb.Mvc.Api.Controllers
{
    public abstract class ApiControllerBase : Controller
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new MethodNotAllowedActionInvoker();
        }
    }
}