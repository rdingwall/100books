using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage.Books;
using Ohb.Mvc.Storage.PreviousReads;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    [Subject("api/v1/books/:id GET")]
    class BooksHttpApiSpecs
    {
        [Tags("Integration")]
        public class when_looking_up_a_book_by_id
        {
            Because of = () => response = ApiClientFactory.NewUser().GetBook("4YydO00I9JYC");

            It should_return_http_200_ok = 
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_be_json = 
                () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

            It should_get_the_books_id =
                () => response.Data.Id.ShouldEqual("4YydO00I9JYC");
            
            It should_get_the_books_title =
                () => response.Data.Title.ShouldEqual("The Google Story");
            
            It should_get_the_books_authors =
                () => response.Data.Authors.ShouldEqual("David A. Vise, Mark Malseed");
            
            It should_get_the_books_publishers =
                () => response.Data.Publisher.ShouldEqual("Delacorte Press");
            
            It should_get_the_books_published_date =
                () => response.Data.PublishedYear.ShouldEqual("2005");

            It should_get_the_books_description =
                () => response.Data.Description.ShouldEqual("\"Here is the story behind one of the most remarkable Internet successes of our time. Based on scrupulous research and extraordinary access to Google, the book takes you inside the creation and growth of a company whose name is a favorite brand and a standard verb recognized around the world. Its stock is worth more than General Motors’ and Ford’s combined, its staff eats for free in a dining room that used to be\u003cb\u003e \u003c/b\u003erun\u003cb\u003e \u003c/b\u003eby the Grateful Dead’s former chef, and its employees traverse the firm’s colorful Silicon Valley campus on scooters and inline skates.\u003cbr\u003e\u003cbr\u003e\u003cb\u003eTHE GOOGLE STORY \u003c/b\u003eis the definitive account of the populist media company powered by the world’s most advanced technology that in a few short years has revolutionized access to information about everything for everybody everywhere. \u003cbr\u003eIn 1998, Moscow-born Sergey Brin and Midwest-born Larry Page dropped out of graduate school at Stanford University to, in their own words, “change the world” through a search engine that would organize every bit of information on the Web for free.\u003cbr\u003e\u003cbr\u003eWhile the company has done exactly that in more than one hundred languages, Google’s quest continues as it seeks to add millions of library books, television broadcasts, and more to its searchable database. \u003cbr\u003eReaders will learn about the amazing business acumen and computer wizardry that started the company on its astonishing course; the secret network of computers delivering lightning-fast search results; the unorthodox approach that has enabled it to challenge Microsoft’s dominance and shake up Wall Street. Even as it rides high, Google wrestles with difficult choices that will enable it to continue expanding while sustaining the guiding vision of its founders’ mantra: DO NO EVIL.\"");
            
            It should_get_the_books_thumbnail_url =
                () => response.Data.ThumbnailUrl.ShouldStartWith("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=1");
            
            It should_not_be_previously_read =
                () => response.Data.HasPreviouslyRead.ShouldBeFalse();

            It should_return_the_isbn_10 =
                () => response.Data.Isbn10.ShouldEqual("055380457X");

            It should_return_the_isbn_13 =
                () => response.Data.Isbn13.ShouldEqual("9780553804577");

            It should_return_the_google_book_link =
                () =>
                response.Data.GoogleBookUrl.ShouldEqual(
                    "http://books.google.co.uk/books/about/The_Google_Story.html?id=4YydO00I9JYC");

            It should_return_the_google_preview_link =
                () => response.Data.GooglePreviewUrl.ShouldEqual(
                    "http://books.google.co.uk/books?id=4YydO00I9JYC&source=gbs_api");

            static RestResponse<BookModel> response;
        }

        [Tags("Integration")]
        public class when_looking_up_a_book_with_a_subtitle
        {
            Because of = () => response = ApiClientFactory.Anonymous().GetBook("a52a_F-OKUcC");

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_concatenate_the_titles =
                () => response.Data.Title.ShouldEqual("Lego: A Love Story");

            static RestResponse<BookModel> response;
        }

        [Tags("Integration")]
        public class when_looking_up_a_book_with_no_covers
        {
            Because of = () => response = ApiClientFactory.Anonymous().GetBook("DAAAAAAACAAJ");

            It should_return_http_200_ok =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_use_the_default_cover_image =
                () => response.Data.ThumbnailUrl.ShouldEqual("img/book-no-cover.png");

            It should_use_the_default_small_cover_image =
                () => response.Data.SmallThumbnailUrl.ShouldEqual("img/search-result-no-cover.png");

            static RestResponse<BookModel> response;
        }

        [Tags("Integration")]
        public class when_no_book_id_is_provided
        {
            It should_return_http_400_bad_request =
                () => new ApiClient().AssertReturns(Method.GET, "books/", HttpStatusCode.BadRequest);
        }

        [Tags("Integration")]
        public class when_no_matching_book_is_found
        {
            It should_return_http_404_not_found =
                () => new ApiClient().AssertReturns(Method.GET, "books/xxxxxxxxxxxxx", HttpStatusCode.NotFound);
        }

        [Tags("Integration")]
        public class when_an_http_post_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.POST, "books/4YydO00I9JYC");
        }

        [Tags("Integration")]
        public class when_an_http_put_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.PUT, "books/4YydO00I9JYC");
        }

        [Tags("Integration")]
        public class when_an_http_delete_is_sent
        {
            It should_return_http_405_method_not_allowed =
                () => new ApiClient().AssertMethodNotAllowed(Method.DELETE, "books/4YydO00I9JYC");
        }

        [Tags("Integration")]
        public class when_getting_a_book_that_was_marked_as_previously_read
        {
            Establish context = () =>
                                    {
                                        api = ApiClientFactory.NewUser();
                                        api.MarkBookAsRead("4YydO00I9JYC");
                                        LiveRavenDb.WaitForNonStaleResults<Book>();
                                        LiveRavenDb.WaitForNonStaleResults<PreviousRead>();
                                    };

            Because of = () => response = api.GetBook("4YydO00I9JYC");

            It should_be_marked_as_previously_read = 
                () => response.Data.HasPreviouslyRead.ShouldBeTrue();

            static ApiClient api;
            static RestResponse<BookModel> response;
        }
    }
}
