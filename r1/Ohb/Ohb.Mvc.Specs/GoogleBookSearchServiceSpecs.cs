using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Google;
using Ohb.Mvc.Models;

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

        public class when_looking_up_a_book_by_id
        {
            Establish context =
                () => service = new GoogleBookSearchService(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs");

            Because of = () => book = service.GetBook("4YydO00I9JYC");

            It should_return_some_results = () => book.ShouldNotBeNull();

            It should_get_the_books_id = () => book.Id.ShouldEqual("4YydO00I9JYC");
            It should_get_the_books_title = () => book.Title.ShouldEqual("The Google story");
            It should_get_the_books_authors = () => book.Authors.ShouldEqual("David A. Vise, Mark Malseed");
            It should_get_the_books_publishers = () => book.Publisher.ShouldEqual("Delacorte Press");
            It should_get_the_books_published_date = () => book.PublishedYear.ShouldEqual("2005");
            It should_get_the_books_description = () => book.Description.ShouldEqual("Here is the story behind one of the most remarkable Internet successes of our time. Based on scrupulous research and extraordinary access to Google, the book takes you inside the creation and growth of a company whose name is a favorite brand and a standard verb recognized around the world. Its stock is worth more than General Motors’ and Ford’s combined, its staff eats for free in a dining room that used to be\u003cb\u003e \u003c/b\u003erun\u003cb\u003e \u003c/b\u003eby the Grateful Dead’s former chef, and its employees traverse the firm’s colorful Silicon Valley campus on scooters and inline skates.\u003cbr\u003e\u003cbr\u003e\u003cb\u003eTHE GOOGLE STORY \u003c/b\u003eis the definitive account of the populist media company powered by the world’s most advanced technology that in a few short years has revolutionized access to information about everything for everybody everywhere. \u003cbr\u003eIn 1998, Moscow-born Sergey Brin and Midwest-born Larry Page dropped out of graduate school at Stanford University to, in their own words, “change the world” through a search engine that would organize every bit of information on the Web for free.\u003cbr\u003e\u003cbr\u003eWhile the company has done exactly that in more than one hundred languages, Google’s quest continues as it seeks to add millions of library books, television broadcasts, and more to its searchable database. \u003cbr\u003eReaders will learn about the amazing business acumen and computer wizardry that started the company on its astonishing course; the secret network of computers delivering lightning-fast search results; the unorthodox approach that has enabled it to challenge Microsoft’s dominance and shake up Wall Street. Even as it rides high, Google wrestles with difficult choices that will enable it to continue expanding while sustaining the guiding vision of its founders’ mantra: DO NO EVIL.");
            It should_get_the_books_thumbnail_url = () => book.ThumbnailUrl.ShouldEqual("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=1&source=gbs_api");
            It should_get_the_books_small_thumbnail_url = () => book.SmallThumbnailUrl.ShouldEqual("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5&source=gbs_api");





            static IBookSearchService service;
            static BigBook book;
        }
    }
}
