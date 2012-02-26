using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsController : OhbApiController
    {
        readonly IBookImporter importer;

        public PreviousReadsController(IBookImporter importer)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            this.importer = importer;
        }

        public IEnumerable<Book> Get()
        {
            return Request.DocumentSession().Query<PreviousRead>().Select(r => r.Book);
        }

        public void Post(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                throw new HttpResponseException("Missing parameter: Google Book Volume ID", HttpStatusCode.BadRequest);

            var book = importer.GetBook(Request.DocumentSession(), id);
            if (book == null)
                throw new HttpResponseException("Book not found (bad Google Book Volume ID?)", HttpStatusCode.NotFound);

            var associationId = "tmpUserId-" + id; // todo: userid+bookid hash

            Request.DocumentSession().Store(new PreviousRead
                                                {
                                                    Book = book
                                                }, associationId);
            Request.DocumentSession().SaveChanges();

        }
    }
}