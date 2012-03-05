using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Api.ActionFilters;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;

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

        [RequiresAuthCookie]
        public IEnumerable<Book> Get()
        {
            return DocumentSession.Query<PreviousRead>().Select(r => r.Book);
        }

        [RequiresAuthCookie]
        public void Post(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            var book = importer.GetBook(DocumentSession, volumeId);
            if (book == null)
                throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                HttpStatusCode.NotFound);

            DocumentSession.Store(new PreviousRead {Book = book});
            DocumentSession.SaveChanges();

        }
    }
}