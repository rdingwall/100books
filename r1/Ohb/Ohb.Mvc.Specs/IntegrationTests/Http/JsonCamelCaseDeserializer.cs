using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

// ReSharper disable CheckNamespace
namespace RestSharp.Deserializers
// ReSharper restore CheckNamespace
{
    public class JsonCamelCaseDeserializer : IDeserializer
    {
        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }

        public T Deserialize<T>(RestResponse response) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(
                response.Content,
                new JsonSerializerSettings
                    {
                        ContractResolver =
                            new CamelCasePropertyNamesContractResolver()
                    });
        }
    }
}