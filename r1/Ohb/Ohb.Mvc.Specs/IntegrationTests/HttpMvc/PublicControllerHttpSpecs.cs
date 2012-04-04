using System.Net;
using Machine.Specifications;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpMvc
{
    [Subject("/FacebookLogin POST")]
    public class PublicControllerHttpSpecs
    {
        public class When_there_is_no_access_token
        {
            Establish context = () =>
                                    {
                                        client = new RestClient("http://localhost");
                                        request = new RestRequest("/FacebookLogin")
                                                      {
                                                          Method = Method.POST
                                                      };
                                    };

            Because of =
                () => response = client.Execute(request);

            It should_return_http_400_bad_request =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.BadRequest);

            static RestResponse response;
            static RestClient client;
            static RestRequest request;
        }

        public class When_it_is_an_invalid_access_token
        {
            Establish context = () =>
            {
                client = new RestClient("http://localhost");
                request = new RestRequest("/FacebookLogin")
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

            Because of =
                () => response = client.Execute(request);

            It should_return_http_401_unauthorized =
                () => response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);

            static RestResponse response;
            static RestClient client;
            static RestRequest request;
        }
    }
}