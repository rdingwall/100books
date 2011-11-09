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
        private readonly IUserContext context;

        public ProfileController(IUserContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            this.context = context;
        }

        public ActionResult Redirect()
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Profile/
        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult Index()
        {
            ViewBag.ProfilePictureUrl = context.User.ProfilePictureUrl;
            ViewBag.Name = context.User.Name;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }

        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult LogOut()
        {
            var fbWebContext = new FacebookWebContext(FacebookApplication.Current, ControllerContext.HttpContext); // or FacebookWebContext.Current;

            fbWebContext.DeleteAuthCookie();

            return RedirectToAction("Index", "Home");
        }
    }
}