using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IUserService {
        void GetUserAsync(string user, Action<User> callback, Action<GitHubException> onError);
        void GetAuthenticatedUserAsync(Action<User> callback, Action<GitHubException> onError);

        void IsFollowingAsync(string user, Action<bool> callback, Action<GitHubException> onError);
        void FollowAsync(string user, Action callback, Action<GitHubException> onError);
        void UnfollowAsync(string user, Action callback, Action<GitHubException> onError);

        void GetFollowersAsync(string user,
                               Action<IEnumerable<User>> callback,
                               Action<GitHubException> onError);
        void GetFollowingAsync(string user,
                               Action<IEnumerable<User>> callback,
                               Action<GitHubException> onError);

        void GetRepositoriesAsync(string user,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<GitHubException> onError);
        void GetWatchedRepositoriesAsync(string user,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<GitHubException> onError);

        void SearchAsync(string query,
                         Action<IEnumerable<User>> callback,
                         Action<GitHubException> onError);
    }
}
