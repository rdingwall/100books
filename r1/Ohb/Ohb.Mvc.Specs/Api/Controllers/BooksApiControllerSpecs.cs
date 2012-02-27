using System;
using System.Net;
using System.Web.Http;
using Machine.Specifications;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Storage;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.Api.Controllers
{
    [Subject(typeof(BooksController))]
    public class BooksApiControllerSpecs
    {
        public abstract class scenario
        {
            Establish context =
                () =>
                {
                    googleVolumeId = "test";

                    importer = MockRepository.GenerateStub<IBookImporter>();
                    documentStore = new StubDocumentStore();
                    book = new Book();

                    controller = new BooksController(importer, documentStore);
                };

            protected static BooksController controller;
            protected static string googleVolumeId;
            protected static IBookImporter importer;
            protected static Book book;
            protected static StubDocumentStore documentStore;
        }

        public class when_getting_a_book : scenario
        {
            static Book result;

            Establish context =
                () => importer.Stub(s => s.GetBook(documentStore.Session, googleVolumeId)).Return(book);

            Because of = () => result = controller.Get(googleVolumeId);

            It should_return_the_book_static_data = 
                () => result.ShouldEqual(book);
        }

        public class when_the_book_doesnt_exist : scenario
        {
            Because of = () => exception = Catch.Exception(() => controller.Get("non-existent-id"));

            It should_throw_an_http_exception = () => exception.ShouldBe(typeof(HttpResponseException));

            It should_return_404_not_found = () => ((HttpResponseException)exception).Response.StatusCode.ShouldEqual(HttpStatusCode.NotFound);

            static Exception exception;
        }

        public class when_no_book_id_is_provided : scenario
        {
            Because of = () => exception = Catch.Exception(() => controller.Get(""));

            It should_throw_an_http_exception = () => exception.ShouldBe(typeof(HttpResponseException));

            It should_return_404_not_found = () => ((HttpResponseException)exception).Response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

            static Exception exception;
        }
    }
}