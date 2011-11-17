using System.Collections.Generic;
using Ohb.Mvc.Amazon;

namespace Ohb.Mvc.Models
{
    public class SearchResultsModel
    {
        public IEnumerable<BookSearchResult> Results { get; set; }
        public string Terms { get; set; }
    }
}