using System;
using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Linq;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Controllers
{
    public class ProfileController : OhbController
    {
        private readonly IUserFactory userFactory;

        public ProfileController(IUserFactory userFactory)
        {
            if (userFactory == null) throw new ArgumentNullException("userFactory");
            this.userFactory = userFactory;
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
            var user = userFactory.GetOrCreateUser(DocumentSession);

            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.Name = user.Name;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }

        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult LogOut()
        {
            FacebookWebContext.Current.DeleteAuthCookie();

            return RedirectToAction("Index", "Home");
        }
    }
}