using System;
using Ohb.Mvc.Google;

namespace Ohb.Mvc.Storage.Books
{
    public class BookStaticInfo
    {
        public string Publisher { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string PublishedYear { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string ThumbnailUrl { get; set; }
        public string SmallThumbnailUrl { get; set; }

        public static BookStaticInfo FromGoogleVolume(GoogleVolume volume)
        {
            if (volume == null) throw new ArgumentNullException("volume");

            return new BookStaticInfo
            {
                Id = volume.Id,
                Title = volume.VolumeInfo.Title,
                SubTitle = volume.VolumeInfo.SubTitle,
                PublishedYear = (volume.VolumeInfo.PublishedDate ?? "").Substring(0, 4),
                Publisher = volume.VolumeInfo.Publisher,
                ThumbnailUrl = volume.VolumeInfo.ImageLinks.Thumbnail,
                SmallThumbnailUrl = volume.VolumeInfo.ImageLinks.SmallThumbnail,
                Authors = String.Join(", ", volume.VolumeInfo.Authors),
                Description = volume.VolumeInfo.Description.Trim('"')
            };
        }
    }
}