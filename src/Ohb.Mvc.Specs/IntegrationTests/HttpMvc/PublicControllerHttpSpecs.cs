using System.Configuration;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Specs.IntegrationTests.HttpApi;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpMvc
{
    [Subject("/fblogin POST"), Tags("Integration")]
    public class PublicControllerHttpSpecs
    {
        public abstract class scenario
        {
            Because of =
                () => response = client.Execute(request).WriteToConsole();

            static protected RestResponse response;
            static protected RestClient client;
            static protected RestRequest request;
        }

        [Subject("/fblogin POST"), Tags("Integration")]
        public class When_there_is_no_access_token : scenario
        {
            Establish context = () =>
                                    {
                                        var url = ConfigurationManager.AppSettings.TestUrl();
                                        client = new RestClient(url);
                                        request = new RestRequest("/fblogin")
                                                      {
                                                          Method = Method.POST
                                                      };
                                    };

            It should_return_http_400_bad_request =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);
        }

        [Subject("/fblogin POST"), Tags("Integration")]
        public class When_it_is_an_invalid_access_token : scenario
        {
            Establish context = () =>
            {
                client = new RestClient("http://localhost");
                request = new RestRequest("/fblogin")
                {
                    Method = Method.POST
                };
                request.Parameters.Add(new Parameter
                                           {
                                               Name = "accessToken",
                                               Type = ParameterType.GetOrPost,
                                               Value = "test"
                                           });
            };

            It should_return_http_401_unauthorized =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }
    }
}