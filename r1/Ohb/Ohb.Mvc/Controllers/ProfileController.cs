using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Linq;

namespace Ohb.Mvc.Controllers
{
    public class ProfileController : OhbController
    {
        public ActionResult Redirect()
        {
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Profile/
        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult Index()
        {
            ViewBag.ProfileImageUrl = User.ProfileImageUrl;
            ViewBag.DisplayName = User.DisplayName;

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