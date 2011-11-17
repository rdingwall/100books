using System;

namespace Ohb.Mvc.Models
{
    public class BookSearchResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string SmallThumbnailUrl { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1} ({2})", Title, Author, Id);
        }
    }
}