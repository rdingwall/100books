using System.Collections.Generic;
using System.Threading.Tasks;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Google
{
    public interface IGoogleBooksClient
    {
        Task<IEnumerable<BookSearchResult>> Search(string terms);
        GoogleVolume GetVolume(string id);
    }
}