using System;
using RestSharp;

namespace NGitHub {
    public interface IGitHubClient {
        ICommitService Commits { get; }
        IFeedService Feeds { get; }
        IIssueService Issues { get; }
        IUserService Users { get; }
        IRepositoryService Repositories { get; }
        IOrganizationService Organizations { get; }

        bool LoggedIn { get; }
        void LoginAsync(string login,
                        string password,
                        Action callback,
                        Action<APICallError> onError);
        void Logout();

        void CallApiAsync<TJsonResponse>(string resource,
                                         API version,
                                         Method method,
                                         Action<TJsonResponse> callback,
                                         Action<APICallError> onError) where TJsonResponse : new();
    }
}
