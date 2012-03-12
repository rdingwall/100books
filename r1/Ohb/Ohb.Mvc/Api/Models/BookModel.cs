using System;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Api.Models
{
    public class BookModel
    {
        // Google books ID
        public string Id { get; set; }

        public bool HasPreviouslyRead { get; set; }

        // Google properties
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string PublishedYear { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string ThumbnailUrl { get; set; }
        public string SmallThumbnailUrl { get; set; }

        public static BookModel FromBook(Book book)
        {
            if (book == null) throw new ArgumentNullException("book");

            var model = new BookModel
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