using System;
using System.Collections.Generic;
using System.Linq;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;
using Ohb.Mvc.Storage.Users;

namespace Ohb.Mvc.Api.Models
{
    public interface IApiModelMapper
    {
        PreviousReadModel ToPreviousRead(PreviousReadWithBook pair);
        BookModel ToBook(Book book);
        ProfileModel ToProfile(User user, IEnumerable<PreviousReadWithBook> previousReads);
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

        public ProfileModel ToProfile(User user, IEnumerable<PreviousReadWithBook> previousReads)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (previousReads == null) throw new ArgumentNullException("previousReads");

            return new ProfileModel
            {
                RecentReads = previousReads.Select(ToPreviousRead).ToList(),
                Id = user.Id,
                ProfileImageUrl = user.ProfileImageUrl,
                DisplayName = user.DisplayName
            };
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
                Title = book.GoogleVolume.VolumeInfo.Title,
                GoogleBookUrl = book.GoogleVolume.VolumeInfo.CanonicalVolumeLink,
                GooglePreviewUrl = book.GoogleVolume.VolumeInfo.PreviewLink
            };

            if (!String.IsNullOrWhiteSpace(book.GoogleVolume.VolumeInfo.PublishedDate))
                model.PublishedYear = book.GoogleVolume.VolumeInfo.PublishedDate.Substring(0, 4);

            if (!String.IsNullOrWhiteSpace(book.GoogleVolume.VolumeInfo.SubTitle))
                model.Title = String.Concat(model.Title, ": ", book.GoogleVolume.VolumeInfo.SubTitle);

            if (book.GoogleVolume.VolumeInfo.IndustryIdentifiers != null)
            {
                model.Isbn10 = book.GoogleVolume.VolumeInfo.IndustryIdentifiers
                    .Where(i => i.Type == "ISBN_10")
                    .Select(
                        i => i.Identifier).FirstOrDefault();
            }

            if (book.GoogleVolume.VolumeInfo.IndustryIdentifiers != null)
            {
                model.Isbn13 = book.GoogleVolume.VolumeInfo.IndustryIdentifiers
                    .Where(i => i.Type == "ISBN_13")
                    .Select(
                        i => i.Identifier).FirstOrDefault();
            }

            // todo: clean this up
            if (String.IsNullOrEmpty(model.ThumbnailUrl))
                model.ThumbnailUrl = "img/book-no-cover.png";

            if (String.IsNullOrWhiteSpace(model.SmallThumbnailUrl))
                model.SmallThumbnailUrl = "img/search-result-no-cover.png";

            return model;
        }
    }
}