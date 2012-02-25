using System;
using System.Web.Mvc;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksApiController : ApiControllerBase
    {
        readonly IBookImporter importer;

        public BooksApiController(IBookImporter importer, IDocumentSession documentSession) : 
            base(documentSession)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            this.importer = importer;
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(400, "Missing parameter: Google Book Volume ID");

            var staticInfo = importer.GetBook(DocumentSession, id);
            if (staticInfo == null)
                return new HttpNotFoundResult("Book not found (bad Google Book Volume ID?)");

            return Json(new BookInfo { StaticInfo = staticInfo }, JsonRequestBehavior.AllowGet);
        }
    }
}