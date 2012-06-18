using RestSharp;
using RestSharp.Deserializers;
using Newtonsoft.Json;

namespace NGitHub.Helpers {
    public interface IRestClientFactory {
        IRestClient CreateRestClient(string baseUrl);
    }

    public class RestClientFactory : IRestClientFactory {
        public IRestClient CreateRestClient(string baseUrl) {
            var restClient = new RestClient(baseUrl);

            restClient.UseSynchronizationContext = false;

            // Just use a lightweight wrapper around Newtonsoft deserialization since
            // we've had problems with RestSharp deserializers in the past.
            restClient.AddHandler(Constants.JsonApplicationContent, new CustomJsonDeserializer());
            restClient.AddHandler(Constants.JsonTextContent, new CustomJsonDeserializer());
            restClient.AddHandler(Constants.XJsonTextContent, new CustomJsonDeserializer());

            return restClient;
        }

        private class CustomJsonDeserializer : IDeserializer {
            private readonly JsonSerializerSettings _settings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            };

            public T Deserialize<T>(IRestResponse response) {
                var result = JsonConvert.DeserializeObject<T>(response.Content, _settings);

                return result;
            }

            public string DateFormat { get; set; }
            public string Namespace { get; set; }
            public string RootElement { get; set; }
        }
    }
}
