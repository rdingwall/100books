using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Ohb.Mvc.Services;

namespace Ohb.Mvc.Google
{
    public class GoogleBookSearchService : IBookSearchService
    {
        readonly string apiKey;

        public GoogleBookSearchService(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            this.apiKey = apiKey;
        }

        public Task<IEnumerable<IBook>> Search(string terms)
        {
            if (terms == null) throw new ArgumentNullException("terms");

            var queryString = HttpUtility.ParseQueryString("");
            queryString["key"] = apiKey;
            queryString["q"] = terms;
            queryString["printType"] = "books";
            queryString["maxResults"] = "20";

            var builder = new UriBuilder("https://www.googleapis.com/books/v1/volumes")
                              {
                                  Query = queryString.ToString()
                              };

            var request = WebRequest.Create(builder.Uri);

            return Task.Factory.FromAsync(
                request.BeginGetResponse,
                r => GetResults(request.EndGetResponse(r)),
                null);
        }

        static IEnumerable<IBook> GetResults(WebResponse response)
        {
            using (response)
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var result = JsonConvert.DeserializeObject<JObject>(reader.ReadToEnd());

                IList<IBook> results = new List<IBook>();

                foreach (var item in result["items"])
                {
                    var volumeInfo = item["volumeInfo"];
                    var title = volumeInfo["title"].Value<string>();

                    if (volumeInfo["subtitle"] != null)
                        title = String.Concat(title, ": ", volumeInfo["subtitle"].Value<string>());

                    var authors = "";
                    if (volumeInfo["authors"] != null)
                        authors = String.Join(", ", volumeInfo["authors"].Values<string>());

                    var thumbnail = "";
                    if (volumeInfo["imageLinks"] != null)
                        thumbnail = volumeInfo["imageLinks"]["thumbnail"].Value<string>();

                    results.Add(new Book(item["id"].Value<string>(),
                        title, authors, thumbnail));
                }

                return results.Where(IgnoreList.IsOkay).Distinct(new BookTitleComparer());
            }
        }
    }
}