using System;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Linq;
using Ohb.Mvc.Services;

namespace Ohb.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUser user;

        public ProfileController(IUser user)
        {
            if (user == null) throw new ArgumentNullException("user");
            this.user = user;
        }

        //
        // GET: /Profile/
        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult Index()
        {
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.Name = user.Name;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }

        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        [HttpPost]
        public ActionResult LogOut()
        {
            var fbWebContext = new FacebookWebContext(FacebookApplication.Current, ControllerContext.HttpContext); // or FacebookWebContext.Current;

            fbWebContext.DeleteAuthCookie();

            return RedirectToAction("Index", "Profile");
        }
    }
}