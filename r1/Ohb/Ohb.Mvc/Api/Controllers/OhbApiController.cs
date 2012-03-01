using System.Web.Http;
using Ohb.Mvc.Storage.Users;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public abstract class OhbApiController : ApiController
    {
        public IDocumentSession DocumentSession { get; set; }
    }
}