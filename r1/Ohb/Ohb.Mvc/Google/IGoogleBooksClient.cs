using System.Collections.Generic;
using System.Threading.Tasks;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Models;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Google
{
    public interface IGoogleBooksClient
    {
        Task<IEnumerable<BookSearchResult>> Search(string terms);
        BookStaticInfo GetVolume(string id);
    }
}