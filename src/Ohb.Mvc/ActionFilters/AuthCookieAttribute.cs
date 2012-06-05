using System;
using System.Web.Mvc;
using Ohb.Mvc.Authentication;
using Ohb.Mvc.Controllers;
using System.Linq;

namespace Ohb.Mvc.ActionFilters
{
    public class AuthCookieAttribute : ActionFilterAttribute
    {
        readonly IAuthCookieFactory cookieFactory;

        public AuthCookieAttribute(IAuthCookieFactory cookieFactory)
        {
            if (cookieFactory == null) throw new ArgumentNullException("cookieFactory");
            this.cookieFactory = cookieFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            if (controller.Request.Cookies.AllKeys.Contains(OhbCookies.AuthCookie))
                return; // cookie already set (what happens if it's expired?)

            if (controller.User == null)
                return; // not logged in

            SendNewAuthCookie(controller);
        }

        void SendNewAuthCookie(OhbController controller)
        {
            var cookie = cookieFactory.CreateAuthCookie(controller.User);
            controller.Response.AppendCookie(cookie);
        }
    }
}