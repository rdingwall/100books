using System;
using System.Linq;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;

namespace Ohb.Mvc.Specs.IntegrationTests.Storage
{
    [Subject(typeof(BookRepository))]
    public class BookRepositorySpecs
    {
        public class When_the_book_was_already_added_to_ravendb
        {
            Establish context = () =>
            {
                TestRavenDb.UseNewTenant();
                using (var session = TestRavenDb.OpenSession())
                {
                    var book = new Book
                    {
                        GoogleVolumeId = "4YydO00I9JYC",
                        StaticInfo =
                            new BookStaticInfo { Title = "First", Id = "4YydO00I9JYC" }
                    };

                    session.Store(new GoogleVolumeId { VolumeId = "4YydO00I9JYC" },
                                  String.Concat("GoogleVolumeIds/", "4YydO00I9JYC"));

                    session.Store(book);
                    session.SaveChanges();

                    // wait for update
                    session.Query<Book>().Customize(a => a.WaitForNonStaleResults()).Any();
                }

                repository = new BookRepository();
            };

            Because of =
                () =>
                {
                    using (var session = TestRavenDb.OpenSession())
                    {
                        repository.Add(new Book
                        {
                            GoogleVolumeId = "4YydO00I9JYC",
                            StaticInfo =
                                new BookStaticInfo { Title = "Second", Id = "4YydO00I9JYC" }
                        }, session);

                        book = repository.Get("4YydO00I9JYC", session);
                    }
                };

            It should_return_the_existing_book =
                () => book.StaticInfo.Title.ShouldEqual("First");

            static BookRepository repository;
            static Book book;
        }

        public class When_adding_and_retrieving_a_book
        {
            Establish context =
                () =>
                {
                    TestRavenDb.UseNewTenant();
                    repository = new BookRepository();
                };

            Because of =
                () =>
                {
                    using (var session = TestRavenDb.OpenSession())
                    {
                        repository.Add(
                            new Book
                                {
                                    GoogleVolumeId = "abc",
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
                    using (var session = TestRavenDb.OpenSession())
                    {
                        var book = session.Query<Book>()
                            .Customize(a => a.WaitForNonStaleResults())
                            .FirstOrDefault(b => b.GoogleVolumeId == "abc");

                        book.ShouldNotBeNull();
                        book.StaticInfo.Title.ShouldEqual("The Google story");
                    }
                };

            static BookRepository repository;
            static Book book;
        }
    }
}