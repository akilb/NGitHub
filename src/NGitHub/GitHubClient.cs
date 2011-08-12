using System;
using System.Net;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

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
                               Action<User> callback,
                               Action<APICallError> onError) {
            Requires.ArgumentNotNull(login, "login");
            Requires.ArgumentNotNull(password, "password");

            Logout();

            var authenticator = new HttpBasicAuthenticator(login, password);
            CallApiAsync<UserResult>(new GitHubRequest("/user/show/", API.v2, Method.GET),
                                     authenticator,
                                     u => {
                                         _authenticator = authenticator;
                                         callback(u.User);
                                     },
                                     e => {
                                         onError(e);
                                     });
        }

        public void Logout() {
            _authenticator = new NullAuthenticator();
        }

        public void CallApiAsync<TResponseData>(GitHubRequest request,
                                                Action<TResponseData> callback,
                                                Action<APICallError> onError) where TResponseData : new() {
            CallApiAsync<TResponseData>(request, _authenticator, callback, onError);
        }

        private void CallApiAsync<TResponseData>(GitHubRequest request,
                                                 IAuthenticator authenticator,
                                                 Action<TResponseData> callback,
                                                 Action<APICallError> onError) where TResponseData : new() {
            Requires.ArgumentNotNull(request, "request");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(authenticator, "authenticator");
            Requires.ArgumentNotNull(onError, "onError");

            var restRequest = new RestRequest {
                Resource = request.Resource,
                Method = request.Method.ToRestSharpMethod()
            };
            foreach (var p in request.Parameters) {
                restRequest.AddParameter(p.Name, p.Value);
            }

            var baseUrl = (request.Version == API.v3) ? Constants.ApiV3Url : Constants.ApiV2Url;
            var restClient = _factory.CreateRestClient(baseUrl);
            restClient.Authenticator = authenticator;

            restClient.ExecuteAsync<TResponseData>(
                restRequest,
                r => {
                    var response = new GitHubResponse<TResponseData>(r);

                    if (response.IsError) {
                        onError(new APICallError(r));
                        return;
                    }

                    callback(response.Data);
                });
        }

        private class NullAuthenticator : IAuthenticator {
            public void Authenticate(RestClient client, RestRequest request) {
                // NOOP
            }
        }
    }
}
