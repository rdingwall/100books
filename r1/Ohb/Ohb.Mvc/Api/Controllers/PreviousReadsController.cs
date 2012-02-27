using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Storage;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class PreviousReadsController : OhbApiController
    {
        readonly IBookImporter importer;
        readonly IDocumentStore documentStore;

        public PreviousReadsController(IBookImporter importer, IDocumentStore documentStore)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            this.importer = importer;
            this.documentStore = documentStore;
        }

        public IEnumerable<Book> Get()
        {
            using (var documentSession = documentStore.OpenSession())
                return documentSession.Query<PreviousRead>().Select(r => r.Book);
        }

        public void Post(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            using (var documentSession = documentStore.OpenSession())
            {
                var book = importer.GetBook(documentSession, volumeId);
                if (book == null)
                    throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                    HttpStatusCode.NotFound);

                documentSession.Store(new PreviousRead {Book = book});
                documentSession.SaveChanges();
            }

        }
    }
}