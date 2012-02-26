using System;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Storage;

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

        public Book Get(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException("Missing parameter: Google Book Volume ID", HttpStatusCode.BadRequest);

            var book = importer.GetBook(Request.DocumentSession(), id);
            if (book == null)
                throw new HttpResponseException("Book not found (bad Google Book Volume ID?)", HttpStatusCode.NotFound);

            return book;
        }
    }
}