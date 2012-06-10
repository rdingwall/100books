using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Ohb.Mvc.Authentication;
using Ohb.Mvc.Models;
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

        public ActionResult Version()
        {
            return View(AppVersionModel.Current);
        }

        public ActionResult Error()
        {
            throw new Exception("This is a test exception designed for testing the error page.");
        }

        [HttpPost]
        public ActionResult FbLogin(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return new HttpBadRequestResult("Missing parameter 'accessToken'.");

            User user;
            try
            {
                using (var session = documentStore.OpenSession())
                    user = userFactory.GetOrCreateFacebookUser(session, accessToken);
            }
            catch (FacebookOAuthException e)
            {
                return new HttpUnauthorizedResult(e.Message);
            }
            catch (WebExceptionWrapper)
            {
                return new HttpServiceUnavailableResult("Sorry, we could not authenticate you against Facebook at this time.");
            }
            catch (SocketException)
            {
                return new HttpServiceUnavailableResult("Sorry, we could not authenticate you against Facebook at this time.");
            }

            var cookie = cookieFactory.CreateAuthCookie(user);
            Response.AppendCookie(cookie);

            return Redirect("/");
        }
    }
}
