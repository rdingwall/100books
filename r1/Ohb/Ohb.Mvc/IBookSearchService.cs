using System.Collections.Generic;
using System.Threading.Tasks;
using Ohb.Mvc.Models;

namespace Ohb.Mvc
{
    public interface IBookSearchService
    {
        Task<IEnumerable<IBook>> Search(string terms);
        BookDetails GetBook(string id);
    }
}