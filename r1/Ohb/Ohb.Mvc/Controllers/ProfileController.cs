using System;
using System.Web.Mvc;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Linq;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Controllers
{
    public class ProfileController : Controller
    {
        readonly IDocumentStore documentStore;
        private readonly IUserFactory userFactory;

        public ProfileController(IDocumentStore documentStore, IUserFactory userFactory)
        {
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            if (userFactory == null) throw new ArgumentNullException("userFactory");
            this.documentStore = documentStore;
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
            User user;
            using (var session = documentStore.OpenSession())
                user = userFactory.GetOrCreateUser(session);

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