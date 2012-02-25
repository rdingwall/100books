using System.Web.Mvc;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsApiController : ApiControllerBase
    {
        public PreviousReadsApiController(IDocumentSession documentSession) : base(documentSession)
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            return new EmptyResult();
        }
    }
}