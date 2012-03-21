using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ohb.Mvc.Google
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleVolumesCollection
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

        [JsonProperty("items")]
        public List<GoogleVolume> Items { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleVolume
    {
        public GoogleVolume()
        {
            VolumeInfo = new GoogleVolumeInfo();
            AccessInfo = new GoogleAccessInfo();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("volumeInfo")]
        public GoogleVolumeInfo VolumeInfo { get; set; }

        [JsonProperty("accessInfo")]
        public GoogleAccessInfo AccessInfo { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleVolumeInfo
    {
        public GoogleVolumeInfo()
        {
            Authors = new List<string>();
            ImageLinks = new GoogleImageLinks();
            IndustryIdentifiers = new List<GoogleIndustryIdentifiers>();

            Title = Publisher = PublishedDate = Description = "";
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subTitle")]
        public string SubTitle { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("mainCategory")]
        public string MainCategory { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageLinks")]
        public GoogleImageLinks ImageLinks { get; set; }

        [JsonProperty("canonicalVolumeLink")]
        public string CanonicalVolumeLink { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("industryIdentifiers")]
        public IList<GoogleIndustryIdentifiers> IndustryIdentifiers { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("previewLink")]
        public string PreviewLink { get; set; }

        [JsonProperty("infoLink")]
        public string InfoLink { get; set; }
    }
    
    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleImageLinks
    {
        public GoogleImageLinks()
        {
            Thumbnail = "";
            SmallThumbnail = "";
            Small = "";
            Medium = "";
        }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; set; }

        [JsonProperty("small")]
        public string Small { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }
    }

    public class GoogleIndustryIdentifiers
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class GoogleAccessInfo
    {
        [JsonProperty("webReaderLink")]
        public string WebReaderLink { get; set; }
    }
}