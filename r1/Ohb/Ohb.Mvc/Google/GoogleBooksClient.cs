using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

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

        public GoogleVolume GetVolume(string id)
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
                
                return JsonConvert.DeserializeObject<GoogleVolume>(json);
            }
        }
    }
}