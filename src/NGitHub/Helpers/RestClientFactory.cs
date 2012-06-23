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
            restClient.AddHandler(Constants.JsonApplicationContent, new JsonSerializer());
            restClient.AddHandler(Constants.JsonTextContent, new JsonSerializer());
            restClient.AddHandler(Constants.XJsonTextContent, new JsonSerializer());

            return restClient;
        }
    }
}
