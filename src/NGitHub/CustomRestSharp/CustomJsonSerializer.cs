using Newtonsoft.Json;
using NGitHub.Utility;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace NGitHub.CustomRestSharp {
    /// <summary>
    /// Simple RestSharp-compliant wrapper around Json.Net serialization/deserialization.
    /// </summary>
    internal class CustomJsonSerializer : IDeserializer, ISerializer {
        private readonly JsonSerializerSettings _settings;

        public CustomJsonSerializer() {
            _settings = new JsonSerializerSettings {
                            NullValueHandling = NullValueHandling.Ignore
                        };
            ContentType = "application/json";
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(RestResponse response) where T : new() {
            Requires.ArgumentNotNull(response, "response");

            return JsonConvert.DeserializeObject<T>(response.Content, _settings);
        }

        public string ContentType { get; set; }

        public string Serialize(object obj) {
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}
