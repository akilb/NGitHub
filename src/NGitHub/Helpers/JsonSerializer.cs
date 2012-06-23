using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace NGitHub.Helpers {
    internal class JsonSerializer : ISerializer, IDeserializer {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
            NullValueHandling = NullValueHandling.Ignore
        };

        public JsonSerializer() {
            ContentType = "application/json";
        }

        public T Deserialize<T>(IRestResponse response) {
            var result = JsonConvert.DeserializeObject<T>(response.Content, _settings);

            return result;
        }

        public string DateFormat { get; set; }
        public string Namespace { get; set; }
        public string RootElement { get; set; }

        public string ContentType { get; set; }

        public string Serialize(object obj) {
            var result = JsonConvert.SerializeObject(obj, _settings);

            return result;
        }
    }
}
