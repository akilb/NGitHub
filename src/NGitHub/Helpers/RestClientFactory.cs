using NGitHub.CustomRestSharp;
using RestSharp;

namespace NGitHub.Helpers {
    public interface IRestClientFactory {
        IRestClient CreateRestClient(string baseUrl);
    }

    public class RestClientFactory : IRestClientFactory {
        public IRestClient CreateRestClient(string baseUrl) {
            var restClient = new RestClient(baseUrl);

            restClient.UseSynchronizationContext = false;

            // RestSharp uses a json deserializer that does not use attribute-
            // based deserialization by default. Therefore, we substitute our
            // own deserializer here...
            restClient.AddHandler(Constants.JsonApplicationContent, new CustomJsonDeserializer());
            restClient.AddHandler(Constants.JsonTextContent, new CustomJsonDeserializer());
            restClient.AddHandler(Constants.XJsonTextContent, new CustomJsonDeserializer());

            return restClient;
        }
    }
}
