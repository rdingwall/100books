using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ohb.Mvc
{
    public interface IBookSearchService
    {
        Task<IEnumerable<IBook>> Search(string terms);
    }
}