using System.Web.Mvc;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public abstract class ApiControllerBase : OhbControllerBase
    {
        protected ApiControllerBase(IDocumentSession documentSession) : base(documentSession)
        {
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            return new MethodNotAllowedActionInvoker();
        }
    }
}