using System;
using NGitHub.Services;
using RestSharp;

namespace NGitHub {
    public interface IGitHubClient {
        ICommitService Commits              { get; }
        IFeedService Feeds                  { get; }
        IIssueService Issues                { get; }
        IUserService Users                  { get; }
        IRepositoryService Repositories     { get; }
        IOrganizationService Organizations  { get; }

        IAuthenticator Authenticator { get; set; }

        void CallApiAsync<TResponseData>(GitHubRequest request,
                                         Action<IGitHubResponse<TResponseData>> callback,
                                         Action<GitHubException> onError) where TResponseData : new();
    }
}
