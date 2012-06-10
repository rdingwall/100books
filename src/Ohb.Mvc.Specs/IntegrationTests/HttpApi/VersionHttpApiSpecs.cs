using System.Linq;
using System.Net;
using System.Reflection;
using Machine.Specifications;
using Ohb.Mvc.Models;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    [Subject("api/v1/version GET")]
    class VersionHttpApiSpecs
    {
        [Tags("Integration")]
        public class when_getting_the_app_version
        {
            Because of = () => response = ApiClientFactory.Anonymous().GetVersion();

            It should_return_http_200_ok = 
                () => response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            It should_be_json = 
                () => response.ContentType.ShouldEqual("application/json; charset=utf-8");

            It should_get_the_assembly_version =
                () => response.Data.AppVersion.ShouldEqual(AppVersionModel.Current.AppVersion);
            
            It should_get_the_git_comit =
                () => response.Data.GitCommit.ShouldEqual(AppVersionModel.Current.GitCommit);

            static RestResponse<AppVersionModel> response;
        }
    }
}
