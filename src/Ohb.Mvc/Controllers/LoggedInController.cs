using System;
using System.Web;
using System.Web.Mvc;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Controllers
{
    public class LoggedInController : OhbController
    {
        public ActionResult Index()
        {
            var model = new UserModel
                            {
                                ProfileImageUrl = User.ProfileImageUrl,
                                DisplayName = User.DisplayName
                            };

            return View(model);
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