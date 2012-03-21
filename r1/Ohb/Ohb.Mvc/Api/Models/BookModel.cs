using System;

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
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public string GoogleBookUrl { get; set; }
        public string GooglePreviewUrl { get; set; }
    }
}