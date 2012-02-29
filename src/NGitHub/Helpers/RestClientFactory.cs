using RestSharp;

namespace NGitHub.Helpers {
    public interface IRestClientFactory {
        IRestClient CreateRestClient(string baseUrl);
    }

    public class RestClientFactory : IRestClientFactory {
        public IRestClient CreateRestClient(string baseUrl) {
            var restClient = new RestClient(baseUrl);

            restClient.UseSynchronizationContext = false;

            return restClient;
        }
    }
}
