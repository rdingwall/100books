using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Ohb.Mvc.Controllers
{
    public class LoggedInController : OhbController
    {
        public ActionResult Index()
        {
            ViewBag.ProfileImageUrl = User.ProfileImageUrl;
            ViewBag.DisplayName = User.DisplayName;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }

        public ActionResult LogOut()
        {
            HttpContext.Response.SetCookie(
                new HttpCookie(OhbCookies.AuthCookie)
                    {
                        Expires = DateTime.UtcNow.AddMonths(-1)
                    });

            return RedirectToAction("Index", "Public");
        }
    }
}