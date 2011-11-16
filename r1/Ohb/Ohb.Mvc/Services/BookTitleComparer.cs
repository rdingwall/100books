using System;
using System.Collections.Generic;

namespace Ohb.Mvc.Services
{
    class BookTitleComparer : IEqualityComparer<IBook>
    {
        public bool Equals(IBook x, IBook y)
        {
            if (x == null) return false;
            if (y == null) return false;
            if (ReferenceEquals(x, y)) return true;
            return String.Equals(x.Title, y.Title, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(IBook obj)
        {
            if (obj == null) return 0;

            return obj.Title.ToLower().GetHashCode();
        }
    }
}