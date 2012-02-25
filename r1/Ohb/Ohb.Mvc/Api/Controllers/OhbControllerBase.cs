using System;
using System.Web.Mvc;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public abstract class OhbControllerBase : Controller
    {
        protected OhbControllerBase(IDocumentSession documentSession)
        {
            if (documentSession == null) throw new ArgumentNullException("documentSession");
            DocumentSession = documentSession;
        }

        public IDocumentSession DocumentSession { get; private set; }
    }
}