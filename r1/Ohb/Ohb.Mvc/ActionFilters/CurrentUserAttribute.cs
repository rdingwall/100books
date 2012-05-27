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
        readonly ICurrentUserInfoProvider currentUserProvider;

        public CurrentUserAttribute(IUserRepository users, ICurrentUserInfoProvider currentUserProvider)
        {
            if (users == null) throw new ArgumentNullException("users");
            if (currentUserProvider == null) throw new ArgumentNullException("currentUserProvider");
            this.users = users;
            this.currentUserProvider = currentUserProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            var currentUserInfo = currentUserProvider.GetCurrentUserInfo();

            if (!currentUserInfo.IsAuthenticated)
                return;

            controller.User = users.GetUser(currentUserInfo.UserId, controller.DocumentSession);
        }
    }
}