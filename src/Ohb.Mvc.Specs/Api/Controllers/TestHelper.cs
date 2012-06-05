using System.Net.Http;
using Raven.Client;

namespace Ohb.Mvc.Specs.Api.Controllers
{
    public class TestHelper
    {
        public static HttpRequestMessage RequestWith(IDocumentSession documentSession)
        {
            return new HttpRequestMessage {Properties = {{"DocumentSession", documentSession}}};
        }
    }
}