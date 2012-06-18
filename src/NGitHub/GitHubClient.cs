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


        public GitHubRequestAsyncHandle CallApiAsync(
                                            GitHubRequest request,
                                            Action<IGitHubResponse> callback,
                                            Action<GitHubException> onError) {
            return CallApiAsync<IGitHubResponse, IRestResponse>(
                        request,
                        r => new GitHubResponse(r),
                        (client, req, processResponse) => client.ExecuteAsync(req, processResponse),
                        callback,
                        onError);
        }

        public GitHubRequestAsyncHandle CallApiAsync<TResponseData>(
                                            GitHubRequest request,
                                            Action<IGitHubResponse<TResponseData>> callback,
                                            Action<GitHubException> onError) where TResponseData : new() {
            return CallApiAsync<IGitHubResponse<TResponseData>, IRestResponse<TResponseData>>(
                        request,
                        r => new GitHubResponse<TResponseData>(r),
                        (client, req, processResponse) => client.ExecuteAsync<TResponseData>(req,processResponse),
                        callback,
                        onError);
        }

        private GitHubRequestAsyncHandle CallApiAsync<TGitHubResponse, TRestResponse>(
                                            GitHubRequest request,
                                            Func<TRestResponse, TGitHubResponse> responseFactory,
                                            Func<IRestClient,
                                                 IRestRequest,
                                                 Action<TRestResponse, RestRequestAsyncHandle>,
                                                 RestRequestAsyncHandle> restClientExecFunc,
                                            Action<TGitHubResponse> callback,
                                            Action<GitHubException> onError)
            where TGitHubResponse : IGitHubResponse
            where TRestResponse : IRestResponse {
            Requires.ArgumentNotNull(request, "request");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(onError, "onError");

            var restRequest = new RestRequest {
                Resource = request.Resource,
                Method = request.Method.ToRestSharpMethod(),
                RequestFormat = DataFormat.Json
            };
            foreach (var p in request.Parameters) {
                restRequest.AddParameter(p.Name, p.Value);
            }

            if (request.Body != null) {
                restRequest.AddBody(request.Body);
            }

            var baseUrl = (request.Version == API.v3) ? Constants.ApiV3Url : Constants.ApiV2Url;
            var restClient = _factory.CreateRestClient(baseUrl);
            restClient.Authenticator = Authenticator;

            var handle = restClientExecFunc(
                            restClient,
                            restRequest,
                            (r, h) => {
                                var response = responseFactory(r);

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
