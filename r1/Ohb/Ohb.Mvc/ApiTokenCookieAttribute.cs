using System;
using System.Web;
using System.Web.Mvc;
using Ohb.Mvc.Controllers;
using Ohb.Mvc.Storage.ApiTokens;
using System.Linq;

namespace Ohb.Mvc
{
    public class ApiTokenCookieAttribute : ActionFilterAttribute
    {
        const string CookieName = "ApiToken";
        readonly IApiTokenFactory apiTokens;

        public ApiTokenCookieAttribute(IApiTokenFactory apiTokens)
        {
            if (apiTokens == null) throw new ArgumentNullException("apiTokens");
            this.apiTokens = apiTokens;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as OhbController;
            if (controller == null)
                return;

            if (controller.Request.Cookies.AllKeys.Contains(CookieName))
                return; // cookie already set

            if (controller.User == null)
                return; // not logged in

            SendNewApiToken(controller);
        }

        void SendNewApiToken(OhbController controller)
        {
            var token = apiTokens.CreateApiToken(controller.User.Id,
                                                 controller.DocumentSession);

            var cookie = new HttpCookie(CookieName, token.Token)
                             {
                                 Expires = token.ExpiresAt
                             };

            controller.Response.AppendCookie(cookie);
        }
    }
}