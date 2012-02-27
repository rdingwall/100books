using System;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Storage;
using Raven.Client;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksController : OhbApiController
    {
        readonly IBookImporter importer;
        readonly IDocumentStore documentStore;

        public BooksController(IBookImporter importer, IDocumentStore documentStore)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            if (documentStore == null) throw new ArgumentNullException("documentStore");
            this.importer = importer;
            this.documentStore = documentStore;
        }

        public Book Get(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            Book book;
            using (var documentSession = documentStore.OpenSession())
                book = importer.GetBook(documentSession, volumeId);

            if (book == null)
                    throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                    HttpStatusCode.NotFound);
            return book;
        }
    }
}