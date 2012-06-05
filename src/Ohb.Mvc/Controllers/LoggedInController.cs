using System;
using System.Web;
using System.Web.Mvc;

namespace Ohb.Mvc.Controllers
{
    public class LoggedInController : OhbController
    {
        public ActionResult Index()
        {
            ViewBag.ProfileImageUrl = User.ProfileImageUrl;
            ViewBag.DisplayName = User.DisplayName;

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