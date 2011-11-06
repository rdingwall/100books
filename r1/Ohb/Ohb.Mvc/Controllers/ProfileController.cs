using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Linq;

namespace Ohb.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [FacebookAuthorize(LoginUrl = "/?ReturnUrl=~/Profile")]
        public ActionResult Index()
        {
            var fb = new FacebookWebClient();
            dynamic me = fb.Get("me");

            ViewBag.ProfilePictureUrl = string.Format("http://graph.facebok.com/{0}/picture", me.id);
            ViewBag.Name = me.name;
            ViewBag.FirstName = me.first_name;
            ViewBag.LastName = me.last_name;

            ViewBag.MessagePostSuccess = Request.QueryString.AllKeys.Contains("success") &&
                                         Request.QueryString["success"] == "True";

            return View();
        }
    }
}