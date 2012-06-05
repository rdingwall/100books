using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Machine.Specifications;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Authentication;
using Ohb.Mvc.Storage.Users;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    public class ApiClient
    {
        readonly RestClient client;
        readonly RestClient dynamicClient;
        readonly RestClient backdoorClient;
        public string BaseUrl { get; set; }
        public string AuthCookie { get; set; }

        public string UserId
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AuthCookie))
                    return null;

                using (var encoder = new AuthCookieEncoder(AuthCookieSecretKey.Value))
                {
                    AuthCookieContext context;
                    if (!encoder.TryDecode(AuthCookie, out context))
                        return null;

                    return context.UserId;
                }
            }
        }

        public User User { get; set; }

        public ApiClient()
        {
            BaseUrl = ConfigurationManager.AppSettings.TestUrl() + "/api/v1/";

            client = new RestClient(BaseUrl);
            client.AddHandler("application/json", new JsonCamelCaseDeserializer());

            // Only get one handler per content type so easiest just to have 2
            // clients
            dynamicClient = new RestClient(BaseUrl);
            dynamicClient.AddHandler("application/json", new DynamicJsonDeserializer());

            backdoorClient = new RestClient(ConfigurationManager.AppSettings.TestUrl() + "/api/backdoor");
            dynamicClient.AddHandler("application/json", new JsonCamelCaseDeserializer());
        }

        public RestResponse<object> Get(string path)
        {
            var request = new RestRequest(path);
            return dynamicClient.Execute<object>(request).WriteToConsole();
        }

        public RestResponse<object> Post(string path)
        {
            var request = new RestRequest(path) {Method = Method.POST};
            return dynamicClient.Execute<object>(request).WriteToConsole();
        }

        public void AssertReturns(Method method, string path, HttpStatusCode expectedStatusCode)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            response.WriteToConsole();
            response.StatusCode.ShouldEqual(expectedStatusCode);
        }

        public void AssertMethodNotAllowed(Method method, string path)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            response.WriteToConsole();
            response.StatusCode.ShouldEqual(HttpStatusCode.MethodNotAllowed);
        }

        public void AssertUnauthorized(string path)
        {
            AssertUnauthorized(Method.GET, path);
        }

        public void AssertUnauthorized(Method method, string path)
        {
            var request = new RestRequest(path) { Method = method };
            var response = client.Execute(request);
            response.WriteToConsole();
            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        public RestResponse<BookModel> GetBook(string googleVolumeId)
        {
            var request = new RestRequest(String.Format("books/{0}", googleVolumeId));
            Authorize(request);
            return client.Execute<BookModel>(request).WriteToConsole();
        }

        public RestResponse MarkBookAsRead(string googleVolumeId)
        {
            var path = "previousreads/";
            if (!String.IsNullOrWhiteSpace(googleVolumeId))
                path += googleVolumeId;
            
            var request = new RestRequest(path)
                              {
                                  Method = Method.PUT,
                                  RequestFormat = DataFormat.Json
                              };

            Authorize(request);

            return client.Execute(request).WriteToConsole();
        }

        void Authorize(IRestRequest request)
        {
            if (String.IsNullOrWhiteSpace(AuthCookie))
                return;

            request.AddCookie(OhbCookies.AuthCookie, AuthCookie);
        }

        public RestResponse<List<PreviousReadModel>> GetPreviousReads()
        {
            var request = new RestRequest("previousreads");
            Authorize(request);
            return client.Execute<List<PreviousReadModel>>(request).WriteToConsole();
        }

        public RestResponse<List<BookStatus>> GetBookStatuses(params string[] bookIds)
        {
            var ids = String.Join(",", bookIds);
            var request = new RestRequest(String.Format("books/{0}/statuses", ids));
            Authorize(request);
            return client.Execute<List<BookStatus>>(request).WriteToConsole();
        }

        public RestResponse RemovePreviousRead(string volumeId)
        {
            var request = new RestRequest(String.Format("previousreads/{0}", volumeId))
            {
                Method = Method.DELETE,
                RequestFormat = DataFormat.Json
            };

            Authorize(request);

            return client.Execute(request).WriteToConsole();
        }

        public RestResponse<ProfileModel> GetProfile(string userId)
        {
            var request = new RestRequest("profiles/" + userId);
            return client.Execute<ProfileModel>(request).WriteToConsole();
        }

        public RestResponse<ProfileModel> GetMyProfile()
        {
            var request = new RestRequest("profiles/me");
            Authorize(request);
            return client.Execute<ProfileModel>(request).WriteToConsole();
        }

        public RestResponse BackdoorGetAuthCookie(string userId)
        {
            // Using a separate client so the cookie doesn't affect other tests
            var tempBackdoorClient = new RestClient(backdoorClient.BaseUrl);

            var request = new RestRequest("getauthcookie");
            if (!String.IsNullOrWhiteSpace(userId))
                request.AddParameter("userId", userId);

            return tempBackdoorClient.Execute(request).WriteToConsole();
        }

        public RestResponse<BackdoorCreateUserResponse> BackdoorCreateUser(string displayName, string profileImageUrl, 
            bool setAuthCookie = false)
        {
            var request = new RestRequest("createuser")
                              {
                                  Method = Method.POST
                              };
            request.AddParameter("displayName", displayName);
            request.AddParameter("profileImageUrl", profileImageUrl);
            request.AddParameter("setAuthCookie", setAuthCookie);

            return backdoorClient.Execute<BackdoorCreateUserResponse>(request).WriteToConsole();
        }
    }
}