using Newtonsoft.Json;
using NGitHub.Utility;
using RestSharp;
using RestSharp.Deserializers;

namespace NGitHub.CustomRestSharp {
    /// <summary>
    /// Simple RestSharp-compliant wrapper around Json.Net deserialization.
    /// </summary>
    internal class CustomJsonDeserializer : IDeserializer {
        private readonly JsonSerializerSettings _settings;

        public CustomJsonDeserializer() {
            _settings = new JsonSerializerSettings {
                            NullValueHandling = NullValueHandling.Ignore
                        };
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public T Deserialize<T>(RestResponse response) where T : new() {
            Requires.ArgumentNotNull(response, "response");

            return JsonConvert.DeserializeObject<T>(response.Content, _settings);
        }
    }
}
