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
        readonly ICurrentUserContextProvider provider;

        public CurrentUserAttribute(IUserRepository users, ICurrentUserContextProvider provider)
        {
            if (users == null) throw new ArgumentNullException("users");
            if (provider == null) throw new ArgumentNullException("provider");
            this.users = users;
            this.provider = provider;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            var context = provider.GetCurrentUser();

            if (!context.IsAuthenticated)
                return;

            controller.User = users.GetUser(context.UserId, controller.DocumentSession);
        }
    }
}