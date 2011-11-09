using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Ohb.Mvc.Amazon;

namespace Ohb.Mvc.Specs
{
    [Subject(typeof(AmazonBookSearchService))]
    public class AmazonBookSearchServiceSpecs
    {
        public class when_searching_for_books
        {
            Establish context = () => service = new AmazonBookSearchService(
                accessKeyId: "AKIAJ3XQI6KPX6JBP7SA",
                secretKey: "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");

            Because of = () => results = service.Search("girl with the dragon tattoo").Result.ToList();

            It should_return_some_results = () => results.ShouldNotBeEmpty();

            It should_find_the_book = () => results.Single(b => b.Asin == "");

            It should_not_include_dvds = () => results.FirstOrDefault(b => b.Asin == "B00361GC7A").ShouldBeNull();

            It should_not_include_kindle_editions = () => results.FirstOrDefault(b => b.Asin == "B002RI9ZQ8").ShouldBeNull();

            static IEnumerable<IBook> results;
            static IAmazonBookSearchService service;
        }
    }
}
