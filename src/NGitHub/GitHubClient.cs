using System;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IUserService _users;

        public GitHubClient()
            : this(new RestClientFactory()) {
        }

        internal GitHubClient(IRestClientFactory factory) {
            Requires.ArgumentNotNull(factory, "factory");

            _factory = factory;
            _users = new UserService(this);
        }

        public IUserService Users {
            get {
                return _users;
            }
        }

        public void CallApiAsync<TJsonResponse>(string resource,
                                                Method method,
                                                Action<TJsonResponse> callback,
                                                Action<APICallError> onError)
            where TJsonResponse : new() {
            Requires.ArgumentNotNull(resource, "resource");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(onError, "onError");

            var request = new RestRequest(resource, method);
            var restClient = _factory.CreateRestClient(Constants.ApiV3Url);
            restClient.ExecuteAsync<TJsonResponse>(
                request,
                r => {
                    if (r.ResponseStatus != ResponseStatus.Completed) {
                        // TODO: Error handling...
                        return;
                    }

                    callback(r.Data);
                });
        }

        public interface IRestClientFactory {
            IRestClient CreateRestClient(string baseUrl);
        }

        private class RestClientFactory : IRestClientFactory {
            public IRestClient CreateRestClient(string baseUrl) {
                return new RestClient(baseUrl);
            }
        }

    }
}
