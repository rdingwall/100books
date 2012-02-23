using System.Web.Mvc;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsApiController : ApiControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return new EmptyResult();
        }
    }
}