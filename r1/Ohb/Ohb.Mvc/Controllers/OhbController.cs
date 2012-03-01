using System.Web.Mvc;
using Raven.Client;

namespace Ohb.Mvc.Controllers
{
    public abstract class OhbController : Controller
    {
        public IDocumentSession DocumentSession { get; set; }
    }
}