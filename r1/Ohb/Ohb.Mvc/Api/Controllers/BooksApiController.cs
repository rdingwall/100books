using System;
using System.Web.Mvc;
using Ohb.Mvc.Api.Models;

namespace Ohb.Mvc.Api.Controllers
{
    public class BooksApiController : Controller
    {
        readonly IBookSearchService searchService;

        public BooksApiController(IBookSearchService searchService)
        {
            if (searchService == null) throw new ArgumentNullException("searchService");
            this.searchService = searchService;
        }

        public ActionResult GetBook(string googleVolumeId)
        {
            if (String.IsNullOrWhiteSpace(googleVolumeId))
                return new HttpStatusCodeResult(400, "Must provide book volume ID.");

            var staticInfo = searchService.GetBook(googleVolumeId);
            if (staticInfo == null)
                return new HttpStatusCodeResult(404, "Book not found.");

            return Json(new BookInfo {StaticInfo = staticInfo});
        }
    }
}