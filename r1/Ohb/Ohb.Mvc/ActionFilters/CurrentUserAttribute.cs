using System;
using System.Web.Mvc;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.ActionFilters
{
    public class CurrentUserAttribute : ActionFilterAttribute
    {
        readonly IUserRepository users;

        public CurrentUserAttribute(IUserRepository users)
        {
            if (users == null) throw new ArgumentNullException("users");
            this.users = users;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            if (!OhbUserContext.Current.IsAuthenticated)
                return;

            controller.User = users.GetUser(OhbUserContext.Current.UserId, controller.DocumentSession);
        }
    }
}