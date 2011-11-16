using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Google;

namespace Ohb.Mvc.Specs
{
    [Subject(typeof(GoogleBookSearchService))]
    public class GoogleBookSearchServiceSpecs
    {
        public class when_searching_for_books
        {
            Establish context = 
                () => service = new GoogleBookSearchService(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs");

            Because of = () => results = service.Search("girl with the dragon tattoo").Result.ToList();

            It should_return_some_results = () => results.ShouldNotBeEmpty();

            It should_find_the_book = () => results.Single(b => b.Asin == "VgAg70looxkC");

            It should_populate_the_books_title = 
                () => results.Single(b => b.Asin == "VgAg70looxkC").Title
                    .ShouldEqual("The girl with the dragon tattoo");

            It should_populate_the_books_author = 
                () => results.Single(b => b.Asin == "VgAg70looxkC").Author
                    .ShouldEqual("Stieg Larsson");

            It should_populate_the_books_thumbnail_link = 
                () => results.Single(b => b.Asin == "VgAg70looxkC").CoverImageUrl
                .ShouldEqual("http://bks3.books.google.co.uk/books?id=VgAg70looxkC&printsec=frontcover&img=1&zoom=1&edge=curl&source=gbs_api");

            static IEnumerable<IBook> results;
            static IBookSearchService service;
        }
    }
}
