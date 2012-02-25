using System.Web.Mvc;
using Ohb.Mvc.Api.Models;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsApiController : ApiControllerBase
    {
        public PreviousReadsApiController(IDocumentSession documentSession) : base(documentSession)
        {
        }

        [ActionName("previousreads"), HttpGet]
        public ActionResult Get()
        {
            return new EmptyResult();
        }

        [ActionName("previousreads"), HttpPost]
        public ActionResult Post(VolumeIdModel model)
        {
            return new EmptyResult();
        }
    }
}