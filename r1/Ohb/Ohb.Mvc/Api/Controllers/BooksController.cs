using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksController : OhbApiController
    {
        readonly IBookImporter importer;

        public BooksController(IBookImporter importer)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            this.importer = importer;
        }

        public BookInfo Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException("Missing parameter: Google Book Volume ID", HttpStatusCode.BadRequest);

            var staticInfo = importer.GetBook(Request.DocumentSession(), id);
            if (staticInfo == null)
                throw new HttpResponseException("Book not found (bad Google Book Volume ID?)", HttpStatusCode.NotFound);

            return new BookInfo { StaticInfo = staticInfo };
        }
    }
}