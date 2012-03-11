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

        public BooksController(IBookImporter importer)
        {
            if (importer == null) throw new ArgumentNullException("importer");
            this.importer = importer;
        }

        public BookModel Get(string volumeId)
        {
            if (String.IsNullOrWhiteSpace(volumeId))
                throw new HttpResponseException("Missing parameter: 'volumeId' (Google Book Volume ID)", HttpStatusCode.BadRequest);

            var book = importer.GetBook(DocumentSession, volumeId);

            if (book == null)
                    throw new HttpResponseException("Book not found (bad Google Book Volume ID?)",
                                                    HttpStatusCode.NotFound);

            var result = new BookModel
                             {
                                 Id = book.GoogleVolumeId,
                                 Authors = book.StaticInfo.Authors,
                                 Description = book.StaticInfo.Description,
                                 PageCount = book.StaticInfo.PageCount,
                                 PublishedYear = book.StaticInfo.PublishedYear,
                                 Publisher = book.StaticInfo.Publisher,
                                 SmallThumbnailUrl = book.StaticInfo.SmallThumbnailUrl,
                                 ThumbnailUrl =  book.StaticInfo.ThumbnailUrl,
                                 Title = book.StaticInfo.Title
                             };

            if (!String.IsNullOrWhiteSpace(book.StaticInfo.SubTitle))
                result.Title = String.Concat(result.Title, ": ", book.StaticInfo.SubTitle);

            // todo: clean this up
            if (String.IsNullOrEmpty(result.ThumbnailUrl))
                result.ThumbnailUrl = "img/book-no-cover.png";

            if (String.IsNullOrWhiteSpace(result.SmallThumbnailUrl))
                result.SmallThumbnailUrl = "img/search-result-no-cover.png";

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