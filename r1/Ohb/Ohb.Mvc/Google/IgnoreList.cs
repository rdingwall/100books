using System;
using System.Linq;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Google
{
    public static class IgnoreList
    {
        static readonly string[] ignoreList = new[]
                                               {
                                                   "bundle"
                                               };

        public static bool IsOkay(BookSearchResult bookSearchResult)
        {
            if (bookSearchResult == null) throw new ArgumentNullException("bookSearchResult");

            return !ignoreList.Any(s => bookSearchResult.Title.IndexOf(s, 0, StringComparison.CurrentCultureIgnoreCase) > -1);
        }
    }
}