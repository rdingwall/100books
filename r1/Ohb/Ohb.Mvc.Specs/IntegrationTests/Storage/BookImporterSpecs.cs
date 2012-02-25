using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Google;
using Ohb.Mvc.Storage;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(BookImporter))]
    public class BookImporterSpecs
    {
        public class when_a_book_has_already_been_imported_into_ravendb
        {
            Establish context = () =>
                                    {
                                        TestRavenDb.UseNewTenant();
                                        using (var session = TestRavenDb.OpenSession())
                                        {
                                            var book = new Book
                                                           {
                                                               StaticInfo = new BookStaticInfo {Title = "Dummy"}
                                                           };

                                            session.Store(book, "4YydO00I9JYC");
                                            session.SaveChanges();
                                        }

                                        importer = new BookImporter(new GoogleBooksClient(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs"));
                                    };

            Because of =
                () =>
                {
                    using (var session = TestRavenDb.OpenSession())
                        book = importer.GetBook(session, "4YydO00I9JYC");
                };

            It should_return_the_existing_book =
                () => book.Title.ShouldEqual("Dummy");

            static IBookImporter importer;
            static BookStaticInfo book;
        }

        public class when_the_book_wasnt_found_in_ravendb
        {
            Establish context =
                () =>
                    {
                        TestRavenDb.UseNewTenant();
                        importer = new BookImporter(new GoogleBooksClient(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs"));
                    };

            Because of =
                () =>
                    {
                        using (var session = TestRavenDb.OpenSession())
                            book = importer.GetBook(session, "4YydO00I9JYC");
                    };

            It should_return_the_book_from_google = 
                () => book.Title.ShouldEqual("The Google story");

            It should_add_the_book_to_ravendb =
                () =>
                    {
                        using (var session = TestRavenDb.OpenSession())
                        {
                            var book = session.Load<Book>("4YydO00I9JYC");
                            book.ShouldNotBeNull();
                            book.StaticInfo.Title.ShouldEqual("The Google story");
                        }
                    };

            static IBookImporter importer;
            static BookStaticInfo book;
        }
    }
}