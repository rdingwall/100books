using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Ohb.Mvc.AuthCookies;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Controllers
{
    public class PublicController : Controller
    {
        readonly IAuthCookieFactory cookieFactory;
        readonly IUserFactory userFactory;
        readonly IDocumentStore documentStore;

        public PublicController(
            IAuthCookieFactory cookieFactory, 
            IUserFactory userFactory, 
            IDocumentStore documentStore)
        {
            if (cookieFactory == null) throw new ArgumentNullException("cookieFactory");
            if (userFactory == null) throw new ArgumentNullException("userFactory");
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            this.cookieFactory = cookieFactory;
            this.documentStore = documentStore;
            this.userFactory = userFactory;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FacebookLogin(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new HttpException((int)HttpStatusCode.BadRequest, "Missing parameter 'accessToken'.");

            // What happens if invalid accessToken? Or cannot access Facebook?
            var facebook = new FacebookClient(accessToken);
            User user;
            using (var session = documentStore.OpenSession())
                user = userFactory.GetOrCreateFacebookUser(session, facebook);

            var cookie = cookieFactory.CreateAuthCookie(user);
            Response.AppendCookie(cookie);

            return Redirect("/");
        }
    }
}
