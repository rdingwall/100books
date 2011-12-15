using System.Collections.Generic;
using System.Threading.Tasks;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Models;

namespace Ohb.Mvc
{
    public interface IBookSearchService
    {
        Task<IEnumerable<BookSearchResult>> Search(string terms);
        BookStaticInfo GetBook(string id);
    }
}