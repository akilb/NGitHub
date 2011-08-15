using System;
using NGitHub.Helpers;
using NGitHub.Models;
using NGitHub.Services;
using NGitHub.Utility;
using NGitHub.Web;
using RestSharp;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IResponseProcessor _processor;
        private readonly IUserService _users;
        private readonly IFeedService _feeds;
        private readonly IIssueService _issues;
        private readonly ICommitService _commits;
        private readonly IOrganizationService _organizations;
        private readonly IRepositoryService _repositories;

        private IAuthenticator _authenticator;

        public GitHubClient()
            : this(new RestClientFactory(), new ResponseProcessor()) {
        }

        public GitHubClient(IRestClientFactory factory,
                            IResponseProcessor processor) {
            Requires.ArgumentNotNull(factory, "factory");
            Requires.ArgumentNotNull(processor, "processor");

            _factory = factory;
            _processor = processor;

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
                               Action<GitHubException> onError) {
            Requires.ArgumentNotNull(login, "login");
            Requires.ArgumentNotNull(password, "password");

            Logout();

            var authenticator = new HttpBasicAuthenticator(login, password);
            CallApiAsync<UserResult>(new GitHubRequest("/user/show/", API.v2, NGitHub.Web.Method.GET),
                                     authenticator,
                                     r => {
                                         _authenticator = authenticator;
                                         callback(r.Data.User);
                                     },
                                     e => {
                                         onError(e);
                                     });
        }

        public void Logout() {
            _authenticator = new NullAuthenticator();
        }

        public void CallApiAsync<TResponseData>(GitHubRequest request,
                                                Action<IGitHubResponse<TResponseData>> callback,
                                                Action<GitHubException> onError) where TResponseData : new() {
            CallApiAsync<TResponseData>(request, _authenticator, callback, onError);
        }

        private void CallApiAsync<TResponseData>(GitHubRequest request,
                                                 IAuthenticator authenticator,
                                                 Action<IGitHubResponse<TResponseData>> callback,
                                                 Action<GitHubException> onError) where TResponseData : new() {
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

                    GitHubException ex = null;
                    if (_processor.TryProcessResponseErrors(response, out ex)) {
                        onError(ex);
                        return;
                    }

                    callback(response);
                });
        }

        private class NullAuthenticator : IAuthenticator {
            public void Authenticate(RestClient client, RestRequest request) {
                // NOOP
            }
        }
    }
}
