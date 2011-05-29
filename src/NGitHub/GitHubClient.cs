using System;
using NGitHub.Http;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IUserService _users;
        private readonly IIssueService _issues;

        public GitHubClient()
            : this(new RestClientFactory()) {
        }

        internal GitHubClient(IRestClientFactory factory) {
            Requires.ArgumentNotNull(factory, "factory");

            _factory = factory;
            _users = new UserService(this);
            _issues = new IssueService(this);
        }

        public IUserService Users {
            get {
                return _users;
            }
        }

        public IIssueService Issues {
            get {
                return _issues;
            }
        }

        public void CallApiAsync<TJsonResponse>(string resource,
                                                Method method,
                                                Action<TJsonResponse> callback,
                                                Action<APICallError> onError) where TJsonResponse : new() {
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
                // RestSharp.WindowsPhone currently only executes callbacks on
                // the UI thread.
                // See https://github.com/johnsheehan/RestSharp/pull/126.
                //
                // To workaround this, we'll use a custom Http object that does
                // not explicitly callback on the UI thread...
                var restClient = new RestClient(baseUrl);
                restClient.HttpFactory = new SimpleFactory<CustomHttp>();

                return restClient;
            }
        }
    }
}
