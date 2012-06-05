using System;
using System.Text;
using RestSharp;

namespace Ohb.Mvc.Specs.IntegrationTests.HttpApi
{
    public static class RestClientExtensions
    {
        public static RestResponse WriteToConsole(this RestResponse response)
        {
            ((RestResponseBase) response).WriteToConsole();
            return response;
        }

        public static RestResponseBase WriteToConsole(this RestResponseBase response)
        {
            Console.WriteLine("{0} {1} {2} {3}",
                              (int)response.StatusCode,
                              response.StatusCode,
                              response.Request == null ? "??" : response.Request.Method.ToString(),
                              response.ResponseUri);

            if (response.ErrorException != null)
            {
                Console.WriteLine();
                Console.WriteLine("Response error:");
                Console.WriteLine(response.ErrorException.ToString());
            }


            Console.WriteLine();
            Console.WriteLine("Response headers:");


            foreach (var header in response.Headers)
                Console.WriteLine("   {0} = {1}", header.Name, header.Value);

            Console.WriteLine();
            Console.WriteLine("Response body:");

            var raw = Encoding.UTF8.GetString(response.RawBytes ?? new byte[0]);
            if (!String.IsNullOrWhiteSpace(response.ContentType) && response.ContentType.Contains("application/json"))
                raw = new JsonFormatter().PrettyPrint(raw);
            Console.WriteLine(raw);

            Console.WriteLine(new string('-', 80));

            return response;
        }

        public static RestResponse<T> WriteToConsole<T>(this RestResponse<T> response)
        {
            WriteToConsole((RestResponseBase) response);
            return response;
        }
    }
}
