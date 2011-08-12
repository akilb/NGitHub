using System;
using NGitHub.Models;

namespace NGitHub {
    public interface IGitHubClient {
        ICommitService Commits              { get; }
        IFeedService Feeds                  { get; }
        IIssueService Issues                { get; }
        IUserService Users                  { get; }
        IRepositoryService Repositories     { get; }
        IOrganizationService Organizations  { get; }

        bool LoggedIn { get; }
        void LoginAsync(string login,
                        string password,
                        Action<User> callback,
                        Action<APICallError> onError);
        void Logout();

        void CallApiAsync<TResponseData>(GitHubRequest request,
                                         Action<IGitHubResponse<TResponseData>> callback,
                                         Action<APICallError> onError) where TResponseData : new();
    }
}
