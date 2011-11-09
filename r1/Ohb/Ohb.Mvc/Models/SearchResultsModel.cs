using System.Collections.Generic;
using Ohb.Mvc.Amazon;

namespace Ohb.Mvc.Models
{
    public class SearchResultsModel
    {
        public IEnumerable<IBook> Results { get; set; }
        public string Terms { get; set; }
    }
}