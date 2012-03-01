using System;
using System.Web.Mvc;
using Ohb.Mvc.Controllers;
using Raven.Client;

namespace Ohb.Mvc.ActionFilters
{
    public class RavenDbAttribute : ActionFilterAttribute
    {
        readonly IDocumentStore documentStore;

        public RavenDbAttribute(IDocumentStore documentStore)
        {
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            this.documentStore = documentStore;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            controller.DocumentSession = documentStore.OpenSession();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            controller.DocumentSession.Dispose();
        }
    }
}