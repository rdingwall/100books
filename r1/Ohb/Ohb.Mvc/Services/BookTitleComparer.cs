using System;
using System.Collections.Generic;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Services
{
    class BookTitleComparer : IEqualityComparer<BookSearchResult>
    {
        public bool Equals(BookSearchResult x, BookSearchResult y)
        {
            if (x == null) return false;
            if (y == null) return false;
            if (ReferenceEquals(x, y)) return true;
            return String.Equals(x.Title, y.Title, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(BookSearchResult obj)
        {
            if (obj == null) return 0;

            return obj.Title.ToLower().GetHashCode();
        }
    }
}