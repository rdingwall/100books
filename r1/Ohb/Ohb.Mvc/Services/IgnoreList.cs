using System;
using System.Linq;

namespace Ohb.Mvc.Services
{
    public static class IgnoreList
    {
        static readonly string[] ignoreList = new[]
                                               {
                                                   "bundle"
                                               };

        public static bool IsOkay(IBook book)
        {
            if (book == null) throw new ArgumentNullException("book");

            return !ignoreList.Any(s => book.Title.IndexOf(s, 0, StringComparison.CurrentCultureIgnoreCase) > -1);
        }
    }
}