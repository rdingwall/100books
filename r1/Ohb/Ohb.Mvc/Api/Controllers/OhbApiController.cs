using System;
using System.Web.Http;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public abstract class OhbApiController : ApiController
    {
        //protected OhbApiController() {}

        //// Get DocumentSession from manual ctor injection when running under MSpec
        //protected OhbApiController(IDocumentSession documentSession)
        //{
        //    if (documentSession == null) throw new ArgumentNullException("documentSession");
        //    this.documentSession = documentSession;
        //}

        //readonly IDocumentSession documentSession;

        //public IDocumentSession DocumentSession
        //{
        //    get
        //    {
        //        if (documentSession != null)
        //            return documentSession;
                
        //        if (Request != null)
        //            return (IDocumentSession)Request.Properties["DocumentSession"];

        //        throw new InvalidOperationException("No DocumentSession available!!");
        //    }
        //}
    }
}