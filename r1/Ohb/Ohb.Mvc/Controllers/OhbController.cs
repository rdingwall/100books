using System.Web.Mvc;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Controllers
{
    public abstract class OhbController : Controller
    {
        public IDocumentSession DocumentSession { get; set; }
        public new User User { get; set; }
    }
}