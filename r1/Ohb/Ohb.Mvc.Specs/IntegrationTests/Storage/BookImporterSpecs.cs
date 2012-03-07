using Machine.Specifications;
using Ohb.Mvc.Google;
using Ohb.Mvc.Storage.Books;
using Raven.Client;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(BookImporter))]
    public class BookImporterSpecs
    {
        public abstract class scenario
        {
            Establish context =
                () =>
                    {
                        books = MockRepository.GenerateMock<IBookRepository>();
                        session = MockRepository.GenerateStub<IDocumentSession>();

                        importer =
                            new BookImporter(
                                new GoogleBooksClient(apiKey: "AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs"),
                                books);
                    };

            protected static IBookImporter importer;
            protected static Book book;
            protected static IBookRepository books;
            protected static IDocumentSession session;
        }

        public class when_a_book_has_already_been_imported_into_ravendb : scenario
        {
            Establish context =
                () =>
                    {
                        var book =
                            new Book
                                {
                                    GoogleVolumeId = "4YydO00I9JYC",
                                    GoogleVolumeIdBase64 = ConvertGoogleVolumeId.ToBase64String("4YydO00I9JYC"),
                                    StaticInfo =
                                        new BookStaticInfo {Title = "Dummy", Id = "4YydO00I9JYC"}
                                };

                        books.Stub(b => b.Get(book.GoogleVolumeId, session)).Return(book);
                    };

            Because of =
                () => book = importer.GetBook(session, "4YydO00I9JYC");

            It should_return_the_existing_book =
                () => book.StaticInfo.Title.ShouldEqual("Dummy");
        }

        public class when_the_book_wasnt_found_in_ravendb : scenario
        {
            Because of =
                () => book = importer.GetBook(session, "4YydO00I9JYC");

            It should_return_the_book_from_google = 
                () => book.StaticInfo.Title.ShouldEqual("The Google story");

            It should_add_the_book_to_ravendb =
                () => books.AssertWasCalled(b => b.Add(book, session));
        }

        public class when_importing_a_non_existing_book : scenario
        {
            Because of =
                () => book = importer.GetBook(session, "xxxxxxxxxxxxxxx");

            It should_return_null =
                () => book.ShouldBeNull();

            It should_add_the_book_to_ravendb =
                () => books.AssertWasNotCalled(b => b.Add(Arg<Book>.Is.Anything, Arg.Is(session)));
        }
    }
}