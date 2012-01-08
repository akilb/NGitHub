using System;
using NGitHub.Authentication;
using NGitHub.Helpers;
using NGitHub.Services;
using NGitHub.Utility;
using NGitHub.Web;
using RestSharp;

namespace NGitHub {
    public class GitHubClient : IGitHubClient {
        private readonly IRestClientFactory _factory;
        private readonly IResponseProcessor _processor;
        private readonly IUserService _users;
        private readonly IIssueService _issues;
        private readonly ICommitService _commits;
        private readonly IOrganizationService _organizations;
        private readonly IRepositoryService _repositories;
        private readonly IPullRequestService _pullRequests;

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

            Authenticator = new NullAuthenticator();
            _users = new UserService(this);
            _issues = new IssueService(this);
            _commits = new CommitService(this);
            _repositories = new RepositoryService(this);
            _pullRequests = new PullRequestService(this);
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

        public IPullRequestService PullRequests {
            get {
                return _pullRequests;
            }
        }

        public IAuthenticator Authenticator {
            get {
                return _authenticator;
            }
            set {
                _authenticator = value ?? new NullAuthenticator();
            }
        }

        public GitHubRequestAsyncHandle CallApiAsync<TResponseData>(
                                            GitHubRequest request,
                                            Action<IGitHubResponse<TResponseData>> callback,
                                            Action<GitHubException> onError) where TResponseData : new() {
            Requires.ArgumentNotNull(request, "request");
            Requires.ArgumentNotNull(callback, "callback");
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
            restClient.Authenticator = Authenticator;

            var handle = restClient.ExecuteAsync<TResponseData>(
                            restRequest,
                            (r, h) => {
                                var response = new GitHubResponse<TResponseData>(r);

                                GitHubException ex = null;
                                if (_processor.TryProcessResponseErrors(response, out ex)) {
                                    onError(ex);
                                    return;
                                }

                                callback(response);
                            });

            return new GitHubRequestAsyncHandle(request, handle);
        }
    }
}
