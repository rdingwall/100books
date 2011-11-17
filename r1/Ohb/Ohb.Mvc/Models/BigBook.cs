using System;

namespace Ohb.Mvc.Models
{
    public class BigBook
    {
        public string Publisher { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string PublishedYear { get; set; }
        public string Description { get; set; }
        public int PageCount { get; set; }
        public string ThumbnailUrl { get; set; }
        public string SmallThumbnailUrl { get; set; }
    }
}