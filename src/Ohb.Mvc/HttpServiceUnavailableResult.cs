using System.Net;
using System.Web.Mvc;

namespace Ohb.Mvc
{
    public class HttpServiceUnavailableResult : HttpStatusCodeResult
    {
        public HttpServiceUnavailableResult(string statusDescription)
            : base((int) HttpStatusCode.ServiceUnavailable, statusDescription)
        {
        }
    }
}