using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Api.Models
{
    public class BookModel
    {
        public Book Book { get; set; }
        public bool HasPreviouslyRead { get; set; }
    }
}