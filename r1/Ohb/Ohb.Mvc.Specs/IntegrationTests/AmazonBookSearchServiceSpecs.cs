using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Amazon;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Specs.IntegrationTests
{
    [Subject(typeof(AmazonBookSearchService))]
    public class AmazonBookSearchServiceSpecs
    {
        [Ignore("Not using amazon atm")]
        public class when_searching_for_books
        {
            Establish context = () => service = new AmazonBookSearchService(
                accessKeyId: "AKIAJ3XQI6KPX6JBP7SA",
                secretKey: "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");

            Because of = () => results = service.Search("girl with the dragon tattoo").Result.ToList();

            It should_return_some_results = () => results.ShouldNotBeEmpty();

            It should_find_the_book = () => results.Single(b => b.Id == "");

            It should_not_include_dvds = () => results.FirstOrDefault(b => b.Id == "B00361GC7A").ShouldBeNull();

            It should_not_include_kindle_editions = () => results.FirstOrDefault(b => b.Id == "B002RI9ZQ8").ShouldBeNull();

            static IEnumerable<BookSearchResult> results;
            static IBookSearchService service;
        }
    }
}
