using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ohb.Mvc.Api.Controllers;
using Raven.Client;

namespace Ohb.Mvc.Api.ActionFilters
{
    public class RavenDbApiAttribute : ActionFilterAttribute
    {
        readonly IDocumentStore documentStore;

        public RavenDbApiAttribute(IDocumentStore documentStore)
        {
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            this.documentStore = documentStore;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var controller = actionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            controller.DocumentSession = documentStore.OpenSession();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var controller = actionExecutedContext.ActionContext.ControllerContext.Controller as OhbApiController;
            if (controller == null)
                return;

            controller.DocumentSession.Dispose();
        }
    }
}