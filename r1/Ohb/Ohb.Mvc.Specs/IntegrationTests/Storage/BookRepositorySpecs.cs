using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Storage;
using Ohb.Mvc.Storage.Books;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(BookRepository))]
    public class BookRepositorySpecs
    {
        public class When_the_book_was_already_added_to_ravendb
        {
            Establish context = () =>
            {
                RavenDb.SpinUpNewDatabase();
                using (var session = RavenDb.OpenSession())
                {
                    var book = new Book
                    {
                        GoogleVolumeId = "4YydO00I9JYC",
                        GoogleVolumeIdBase64 = ConvertGoogleVolumeId.ToBase64String("4YydO00I9JYC"),
                        StaticInfo =
                            new BookStaticInfo { Title = "First", Id = "4YydO00I9JYC" }
                    };

                    new RavenUniqueInserter().StoreUnique(session, book, b => b.GoogleVolumeIdBase64);

                    session.SaveChanges();

                    // wait for update
                    session.Query<Book>().Customize(a => a.WaitForNonStaleResults()).Any();
                }

                repository = new BookRepository(new RavenUniqueInserter());
            };

            Because of =
                () =>
                {
                    using (var session = RavenDb.OpenSession())
                    {
                        repository.Add(new Book
                        {
                            GoogleVolumeId = "4YydO00I9JYC",
                            GoogleVolumeIdBase64 = ConvertGoogleVolumeId.ToBase64String("4YydO00I9JYC"),
                            StaticInfo =
                                new BookStaticInfo { Title = "Second", Id = "4YydO00I9JYC" }
                        }, session);

                        book = repository.Get("4YydO00I9JYC", session);
                    }
                };

            It should_return_the_existing_book =
                () => book.StaticInfo.Title.ShouldEqual("First");

            static IBookRepository repository;
            static Book book;
        }

        public class When_adding_and_retrieving_a_book
        {
            Establish context =
                () =>
                {
                    RavenDb.SpinUpNewDatabase();
                    repository = new BookRepository(new RavenUniqueInserter());
                };

            Because of =
                () =>
                {
                    using (var session = RavenDb.OpenSession())
                    {
                        repository.Add(
                            new Book
                                {
                                    GoogleVolumeId = "abc",
                                    GoogleVolumeIdBase64 = ConvertGoogleVolumeId.ToBase64String("abc"),
                                    StaticInfo =
                                        new BookStaticInfo { Title = "The Google story", Id = "abc" }
                                }, session);

                        // wait
                        session.Query<Book>()
                            .Customize(a => a.WaitForNonStaleResults())
                            .Any();

                        book = repository.Get("abc", session);
                    }
                };

            It should_be_able_to_retrieve_the_book =
                () => book.StaticInfo.Title.ShouldEqual("The Google story");

            It should_add_the_book_to_ravendb =
                () =>
                {
                    using (var session = RavenDb.OpenSession())
                    {
                        var book = session.Query<Book>()
                            .Customize(a => a.WaitForNonStaleResults())
                            .FirstOrDefault(b => b.GoogleVolumeIdBase64 == ConvertGoogleVolumeId.ToBase64String("abc"));

                        book.ShouldNotBeNull();
                        book.StaticInfo.Title.ShouldEqual("The Google story");
                    }
                };

            static IBookRepository repository;
            static Book book;
        }

        public class when_adding_books_with_the_same_google_volume_id_but_different_cased_letters
        {
            Establish context =
                () =>
                    {
                        repository = new BookRepository(new RavenUniqueInserter());

                        using (var session = RavenDb.OpenSession())
                        {
                            book1 = new Book
                                        {
                                            GoogleVolumeId = "aaa",
                                            GoogleVolumeIdBase64 =
                                                ConvertGoogleVolumeId.ToBase64String("aaa"),
                                            StaticInfo = new BookStaticInfo {Title = "Book1"}
                                        };

                            book2 = new Book
                                        {
                                            GoogleVolumeId = "aAa",
                                            GoogleVolumeIdBase64 =
                                                ConvertGoogleVolumeId.ToBase64String("aAa"),
                                            StaticInfo = new BookStaticInfo {Title = "Book2"}
                                        };

                            repository.Add(book1, session);
                            repository.Add(book2, session);
                            RavenDb.WaitForNonStaleResults<Book>();
                        }
                    };

            Because of =
                () =>
                    {
                        using (var session = RavenDb.OpenSession())
                        {
                            returnedBook1 = repository.Get("aaa", session);
                            returnedBook2 = repository.Get("aAa", session);
                        }
                    };

            It should_return_the_correct_first_book =
                () => returnedBook1.StaticInfo.Title.ShouldEqual(book1.StaticInfo.Title);

            It should_return_the_correct_second_book =
                () => returnedBook2.StaticInfo.Title.ShouldEqual(book2.StaticInfo.Title);

            static IBookRepository repository;
            static Book book1;
            static Book book2;
            static Book returnedBook1;
            static Book returnedBook2;
        }
    }
}