using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ohb.Mvc.Google
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleVolume
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("volumeInfo")]
        public GoogleVolumeInfo VolumeInfo { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleVolumeInfo
    {
        public GoogleVolumeInfo()
        {
            Authors = new List<string>();
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subTitle")]
        public string SubTitle { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageLinks")]
        public GoogleImageLinks ImageLinks { get; set; }
    }
    
    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleImageLinks
    {
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; set; }
    }
}