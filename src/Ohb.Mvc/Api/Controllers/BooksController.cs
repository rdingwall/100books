using System;
using System.Net;
using System.Web.Http;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksController : OhbApiController
    {
        readonly IBookImporter importer;
        readonly IApiModelMapper mapper;

        public BooksController(IBookImporter importer, IApiModelMapper mapper)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            if (mapper == null) throw new ArgumentNullException("mapper");
            this.importer = importer;
            this.mapper = mapper;
        }

        public BookModel Get(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            var book = importer.GetBook(DocumentSession, volumeId);

            if (book == null)
                    throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                    HttpStatusCode.NotFound);

            var result = mapper.ToBook(book);

            if (User != null)
            {
                var previousRead = DocumentSession.Load<PreviousRead>(PreviousRead.MakeId(User.Id, volumeId));

                if (previousRead != null)
                    result.HasPreviouslyRead = true;
            }

            return result;
        }
    }
}