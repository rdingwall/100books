using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Machine.Specifications;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;
using Raven.Client;
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
                    documentSession = MockRepository.GenerateStub<IDocumentSession>();
                    bookStaticInfo = new BookStaticInfo();

                    controller = new BooksController(importer)
                                     {
                                         Request = TestHelper.RequestWith(documentSession)
                                     };
                    ;
                };

            protected static BooksController controller;
            protected static string googleVolumeId;
            protected static IBookImporter importer;
            protected static BookStaticInfo bookStaticInfo;
            protected static IDocumentSession documentSession;
        }

        public class when_getting_a_book : scenario
        {
            static BookInfo result;

            Establish context =
                () => importer.Stub(s => s.GetBook(documentSession, googleVolumeId)).Return(bookStaticInfo);

            Because of = () => result = controller.Get(googleVolumeId);

            It should_return_the_book_static_data = 
                () => result.StaticInfo.ShouldEqual(bookStaticInfo);

            It should_be_unread =
                () => result.HasPreviouslyRead.ShouldBeFalse();
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