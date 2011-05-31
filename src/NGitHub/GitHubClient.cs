using System;
using NGitHub.CustomRestSharp;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;
using Newtonsoft.Json;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IUserService _users;
        private readonly IIssueService _issues;
        private readonly ICommentService _comments;
        private readonly ICommitService _commits;
        private readonly IOrganizationService _organizations;
        private readonly IRepositoryService _repositories;

        private IAuthenticator _authenticator;

        public GitHubClient()
            : this(new RestClientFactory()) {
        }

        internal GitHubClient(IRestClientFactory factory) {
            Requires.ArgumentNotNull(factory, "factory");

            _factory = factory;

            _authenticator = new NullAuthenticator();
            _users = new UserService(this);
            _issues = new IssueService(this);
            _comments = new CommentService(this);
            _commits = new CommitService(this);
            _repositories = new RepositoryService(this);
            _organizations = new OrganizationService(this);
        }

        public IUserService Users {
            get {
                return _users;
            }
        }

        public IOrganizationService Organizations {
            get {
                return _organizations;
            }
        }

        public IIssueService Issues {
            get {
                return _issues;
            }
        }

        public ICommentService Comments {
            get {
                return _comments;
            }
        }

        public ICommitService Commits {
            get {
                return _commits;
            }
        }

        public IRepositoryService Repositories {
            get {
                return _repositories;
            }
        }

        public bool LoggedIn {
            get {
                return !(_authenticator is NullAuthenticator);
            }
        }

        public void LoginAsync(string login,
                               string password,
                               Action callback,
                               Action<APICallError> onError) {
            Requires.ArgumentNotNull(login, "login");
            Requires.ArgumentNotNull(password, "password");

            _authenticator = new NullAuthenticator();
            var authenticator = new HttpBasicAuthenticator(login, password);
            CallApiAsync<User>("/user",
                               API.v3,
                               Method.GET,
                               authenticator,
                               u => {
                                   _authenticator = authenticator;
                                   callback();
                               },
                               e => {
                                   onError(e);
                               });
        }

        public void Logout() {
            _authenticator = new NullAuthenticator();
        }

        public void CallApiAsync<TJsonResponse>(string resource,
                                                API version,
                                                Method method,
                                                Action<TJsonResponse> callback,
                                                Action<APICallError> onError) where TJsonResponse : new() {
            CallApiAsync<TJsonResponse>(resource,
                                        version,
                                        method,
                                        _authenticator,
                                        callback,
                                        onError);
        }

        private void CallApiAsync<TJsonResponse>(string resource,
                                                 API version,
                                                 Method method,
                                                 IAuthenticator authenticator,
                                                 Action<TJsonResponse> callback,
                                                 Action<APICallError> onError) where TJsonResponse : new() {
            Requires.ArgumentNotNull(resource, "resource");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(authenticator, "authenticator");
            Requires.ArgumentNotNull(onError, "onError");

            var baseUrl = (version == API.v3) ? Constants.ApiV3Url : Constants.ApiV2Url;
            var restClient = _factory.CreateRestClient(baseUrl);
            restClient.Authenticator = authenticator;

            var request = new RestRequest(resource, method);
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

        private class NullAuthenticator : IAuthenticator {
            public void Authenticate(RestClient client, RestRequest request) {
                // NOOP
            }
        }

        public interface IRestClientFactory {
            IRestClient CreateRestClient(string baseUrl);
        }

        private class RestClientFactory : IRestClientFactory {
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
}
