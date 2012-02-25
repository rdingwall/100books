using System.Web.Mvc;
using Machine.Specifications;
using Ohb.Mvc.Api.Controllers;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Storage;
using Raven.Client;
using Rhino.Mocks;

namespace Ohb.Mvc.Specs.Api.Controllers
{
    [Subject(typeof(BooksApiController))]
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

                    controller = new BooksApiController(importer, documentSession);
                };

            protected static BooksApiController controller;
            protected static string googleVolumeId;
            protected static IBookImporter importer;
            protected static BookStaticInfo bookStaticInfo;
            protected static IDocumentSession documentSession;
        }

        public class when_getting_a_book : scenario
        {
            static JsonResult result;

            Establish context =
                () => importer.Stub(s => s.GetBook(documentSession, googleVolumeId)).Return(bookStaticInfo);

            Because of = () => result = (JsonResult)controller.Get(googleVolumeId);

            It should_allow_get =
                () => result.JsonRequestBehavior.ShouldEqual(JsonRequestBehavior.AllowGet);

            It should_return_the_book = 
                () => result.Data.ShouldBe(typeof(BookInfo));

            It should_return_the_book_static_data = 
                () => ((BookInfo) result.Data).StaticInfo.ShouldEqual(bookStaticInfo);

            It should_be_unread =
                () => ((BookInfo) result.Data).HasPreviouslyRead.ShouldBeFalse();
        }

        public class when_the_book_doesnt_exist : scenario
        {
            Because of = () => result = (HttpStatusCodeResult)controller.Get("non-existent-id");

            It should_return_404 = () => result.StatusCode.ShouldEqual(404);

            static HttpStatusCodeResult result;
        }

        public class when_no_book_id_is_provided : scenario
        {
            Because of = () => result = (HttpStatusCodeResult)controller.Get("");

            It should_return_400 = () => result.StatusCode.ShouldEqual(400);

            static HttpStatusCodeResult result;
        }
    }
}