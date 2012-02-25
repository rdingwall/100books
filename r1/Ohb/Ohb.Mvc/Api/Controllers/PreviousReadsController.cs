using System.Web.Mvc;
using Ohb.Mvc.Api.Models;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsController : OhbApiController
    {
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