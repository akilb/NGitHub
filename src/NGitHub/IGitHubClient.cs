using System;
using NGitHub.Services;
using RestSharp;

namespace NGitHub {
    public interface IGitHubClient {
        IIssueService Issues                { get; }
        IUserService Users                  { get; }
        IRepositoryService Repositories     { get; }
        IPullRequestService PullRequests    { get; }
        IOrganizationService Organizations  { get; }

        IAuthenticator Authenticator { get; set; }

        GitHubRequestAsyncHandle CallApiAsync<TResponseData>(
                                    GitHubRequest request,
                                    Action<IGitHubResponse<TResponseData>> callback,
                                    Action<GitHubException> onError) where TResponseData : new();
    }
}
