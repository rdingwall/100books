using System;
using System.Web.Mvc;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc
{
    public class CurrentUserAttribute : ActionFilterAttribute
    {
        readonly IUserFactory userFactory;

        public CurrentUserAttribute(IUserFactory userFactory)
        {
            if (userFactory == null) throw new ArgumentNullException("userFactory");
            this.userFactory = userFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            controller.User = userFactory.GetOrCreateUser(controller.DocumentSession);
        }
    }
}