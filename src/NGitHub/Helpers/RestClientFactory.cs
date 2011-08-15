using NGitHub.CustomRestSharp;
using RestSharp;

namespace NGitHub.Helpers {
    public interface IRestClientFactory {
        IRestClient CreateRestClient(string baseUrl);
    }

    public class RestClientFactory : IRestClientFactory {
        public IRestClient CreateRestClient(string baseUrl) {
            var restClient = new RestClient(baseUrl);

            // RestSharp.WindowsPhone currently only executes callbacks on
            // the UI thread.
            // See https://github.com/johnsheehan/RestSharp/pull/126.
            //
            // To workaround this, we'll use a custom Http object that does
            // not explicitly callback on the UI thread...
            restClient.HttpFactory = new SimpleFactory<CustomHttp>();

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
