using System;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;

namespace Ohb.Mvc.Api.Models
{
    public interface IApiModelMapper
    {
        PreviousReadModel ToPreviousRead(PreviousReadWithBook pair);
        BookModel ToBook(Book book);
    }

    public class ApiModelMapper : IApiModelMapper
    {
        public PreviousReadModel ToPreviousRead(PreviousReadWithBook pair)
        {
            if (pair == null) throw new ArgumentNullException("pair");

            var model = ToBookBase<PreviousReadModel>(pair.Book);
            model.MarkedByUserAt = pair.MarkedByUserAt;
            return model;
        }

        public BookModel ToBook(Book book)
        {
            return ToBookBase<BookModel>(book);
        }

        static T ToBookBase<T>(Book book) where T : BookModel, new()
        {
            if (book == null) throw new ArgumentNullException("book");

            var model = new T
            {
                Id = book.GoogleVolumeId,
                Authors = String.Join(", ", book.GoogleVolume.VolumeInfo.Authors),
                Description = book.GoogleVolume.VolumeInfo.Description,
                PageCount = book.GoogleVolume.VolumeInfo.PageCount,
                Publisher = book.GoogleVolume.VolumeInfo.Publisher,
                SmallThumbnailUrl = book.GoogleVolume.VolumeInfo.ImageLinks.SmallThumbnail,
                ThumbnailUrl = book.GoogleVolume.VolumeInfo.ImageLinks.Thumbnail,
                Title = book.GoogleVolume.VolumeInfo.Title
            };

            if (!String.IsNullOrWhiteSpace(book.GoogleVolume.VolumeInfo.PublishedDate))
                model.PublishedYear = book.GoogleVolume.VolumeInfo.PublishedDate.Substring(0, 4);

            if (!String.IsNullOrWhiteSpace(book.GoogleVolume.VolumeInfo.SubTitle))
                model.Title = String.Concat(model.Title, ": ", book.GoogleVolume.VolumeInfo.SubTitle);

            // todo: clean this up
            if (String.IsNullOrEmpty(model.ThumbnailUrl))
                model.ThumbnailUrl = "img/book-no-cover.png";

            if (String.IsNullOrWhiteSpace(model.SmallThumbnailUrl))
                model.SmallThumbnailUrl = "img/search-result-no-cover.png";

            return model;
        }
    }
}