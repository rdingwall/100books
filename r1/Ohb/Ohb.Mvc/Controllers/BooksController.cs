using System;
using System.Web.Mvc;

namespace Ohb.Mvc.Controllers
{
    public class BooksController : Controller
    {
        readonly IBookSearchService books;

        public BooksController(IBookSearchService books)
        {
            if (books == null) throw new ArgumentNullException("books");
            this.books = books;
        }

        public ActionResult Get(string id)
        {
            var book = books.GetBook(id);
            return View(book);
        }
    }
}