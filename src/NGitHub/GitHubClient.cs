using System;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IUserService _users;
        private readonly IFeedService _feeds;
        private readonly IIssueService _issues;
        private readonly ICommitService _commits;
        private readonly IOrganizationService _organizations;
        private readonly IRepositoryService _repositories;

        private IAuthenticator _authenticator;

        public GitHubClient()
            : this(new RestClientFactory()) {
        }

        public GitHubClient(IRestClientFactory factory) {
            Requires.ArgumentNotNull(factory, "factory");

            _factory = factory;

            _authenticator = new NullAuthenticator();
            _users = new UserService(this);
            _issues = new IssueService(this);
            _commits = new CommitService(this);
            _repositories = new RepositoryService(this);
            _organizations = new OrganizationService(this);

            _feeds = new FeedService(_factory, () => _authenticator);
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

        public IFeedService Feeds {
            get {
                return _feeds;
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

            Logout();

            var authenticator = new HttpBasicAuthenticator(login, password);
            CallApiAsync<UserResult>("/user/show/",
                                     API.v2,
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
    }
}
