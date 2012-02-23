using System;
using System.Web.Mvc;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Google;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksApiController : Controller
    {
        readonly IGoogleBooksClient searchService;

        public BooksApiController(IGoogleBooksClient searchService)
        {
            if (searchService == null) throw new ArgumentNullException("searchService");
            this.searchService = searchService;
        }

        public ActionResult GetBook(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new HttpStatusCodeResult(400, "Missing parameter: Google Book Volume ID");

            var staticInfo = searchService.GetVolume(id);
            if (staticInfo == null)
                return new HttpNotFoundResult("Book not found (bad Google Book Volume ID?)");

            return Json(new BookInfo { StaticInfo = staticInfo }, JsonRequestBehavior.AllowGet);
        }
    }
}