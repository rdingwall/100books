using System.Net;
using System.Web.Mvc;

namespace Ohb.Mvc
{
    public class HttpBadRequestResult : HttpStatusCodeResult
    {
        public HttpBadRequestResult(string statusDescription)
            : base((int) HttpStatusCode.BadRequest, statusDescription)
        {
        }
    }
}