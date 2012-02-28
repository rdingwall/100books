using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Linq;
using Ohb.Mvc.Api.Models;
using Ohb.Mvc.Models;

namespace Ohb.Mvc.Google
{
    public class GoogleBooksClient : IGoogleBooksClient
    {
        readonly string apiKey;

        public GoogleBooksClient(string apiKey)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            this.apiKey = apiKey;
        }

        public Task<IEnumerable<BookSearchResult>> Search(string terms)
        {
            if (terms == null) throw new ArgumentNullException("terms");

            var queryString = HttpUtility.ParseQueryString("");
            queryString["key"] = apiKey;
            queryString["q"] = terms;
            queryString["printType"] = "books";
            queryString["maxResults"] = "20";
            
            if (HttpContext.Current != null)
                queryString["userIp"] = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

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

        public BookStaticInfo GetVolume(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            using (var client = new WebClient { Encoding = Encoding.UTF8 })
            {
                string json = null;
                try
                {
                    json = client.DownloadString(String.Format("https://www.googleapis.com/books/v1/volumes/{0}", id));

                }
                catch (WebException e)
                {
                    if (!(e.Response is HttpWebResponse))
                        throw;

                    var response = e.Response as HttpWebResponse;

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return null;
                }

                if (String.IsNullOrWhiteSpace(json))
                    throw new InvalidOperationException(String.Format("Expected JSON but got '{0}'.", json));
                
                var volume = JsonConvert.DeserializeObject<GoogleVolume>(json);

                return new BookStaticInfo
                           {
                               Id = volume.Id,
                               Title = volume.VolumeInfo.Title,
                               PublishedYear = (volume.VolumeInfo.PublishedDate ?? "").Substring(0, 4),
                               Publisher = volume.VolumeInfo.Publisher,
                               ThumbnailUrl = volume.VolumeInfo.ImageLinks.Thumbnail,
                               SmallThumbnailUrl = volume.VolumeInfo.ImageLinks.SmallThumbnail,
                               Authors = String.Join(", ", volume.VolumeInfo.Authors),
                               Description = volume.VolumeInfo.Description.Trim('"')
                           };
            }
        }

        static IEnumerable<BookSearchResult> GetResults(WebResponse response)
        {
            using (response)
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var result = JsonConvert.DeserializeObject<GoogleVolumesCollection>(reader.ReadToEnd());

                return result.Items
                    .Select(volume => new BookSearchResult
                                          {
                                              Id = volume.Id,
                                              Title = volume.VolumeInfo.Title,
                                              Author = String.Join(", ", volume.VolumeInfo.Authors),
                                              SmallThumbnailUrl = volume.VolumeInfo.ImageLinks.SmallThumbnail
                                          })
                    .ToList()
                    .Where(IgnoreList.IsOkay)
                    .Distinct(new BookTitleComparer());
            }
        }
    }
}