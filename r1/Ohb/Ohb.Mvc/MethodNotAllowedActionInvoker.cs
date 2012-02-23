using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ohb.Mvc
{
    // return HTTP 405 method not allowed instead of 404
    // from http://stackoverflow.com/a/5199156/91551
    class MethodNotAllowedActionInvoker : ControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            // Find action, use selector attributes
            var action = base.FindAction(controllerContext, controllerDescriptor, actionName);

            if (action == null)
            {
                // Find action, ignore selector attributes
                var action2 = controllerDescriptor
                    .GetCanonicalActions()
                    .FirstOrDefault(a => a.ActionName.Equals(actionName, StringComparison.OrdinalIgnoreCase));

                if (action2 != null)
                {
                    // Action found, Method Not Allowed ?
                    throw new HttpException(405, "Method Not Allowed");
                }
            }

            return action;
        }
    }
}