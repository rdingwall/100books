using System;
using System.Web.Mvc;
using Ohb.Mvc.Google;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Controllers
{
    public class SearchController : Controller
    {
        readonly IGoogleBooksClient searchService;

        public SearchController(IGoogleBooksClient searchService)
        {
            if (searchService == null) throw new ArgumentNullException("searchService");
            this.searchService = searchService;
        }

        [HttpGet]
        public ActionResult Search(string query)
        {
            var results = searchService.Search(query).Result;
            return View(new SearchResultsModel
                            {
                                Results = results,
                                Terms = query
                            });
        }
    }
}