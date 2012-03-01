using System;
using System.Web.Mvc;
using Ohb.Mvc.Google;

namespace Ohb.Mvc.Controllers
{
    public class BooksController : OhbController
    {
        readonly IGoogleBooksClient books;

        public BooksController(IGoogleBooksClient books)
        {
            if (books == null) throw new ArgumentNullException("books");
            this.books = books;
        }

        public ActionResult Get(string id)
        {
            var book = books.GetVolume(id);
            return View(book);
        }
    }
}