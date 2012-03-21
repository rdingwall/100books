using System.Web.Mvc;
using Facebook.Web;

namespace Ohb.Mvc.Controllers
{
    public class PublicController : OhbController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LogOn(string returnUrl)
        {
            var fbWebContext = FacebookWebContext.Current;
            
            if (fbWebContext.IsAuthorized())
            {
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return new RedirectResult(returnUrl);
                    }
                }

                return RedirectToAction("Index", "LoggedIn");
            }

            return View();
        }
    }
}
