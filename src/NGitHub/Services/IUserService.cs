using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IUserService {
        void GetUserAsync(string user, Action<User> callback, Action<GitHubException> onError);

        void GetAuthenticatedUserAsync(Action<User> callback, Action<GitHubException> onError);

        void GetFollowersAsync(string user,
                               int page,
                               Action<IEnumerable<User>> callback,
                               Action<GitHubException> onError);

        void GetFollowingAsync(string user,
                               int page,
                               Action<IEnumerable<User>> callback,
                               Action<GitHubException> onError);

        void FollowAsync(string user, Action callback, Action<GitHubException> onError);

        void UnfollowAsync(string user, Action callback, Action<GitHubException> onError);

        void IsFollowingAsync(string user,
                              Action<bool> callback,
                              Action<GitHubException> onError);

        void GetWatchersAsync(string user,
                              string repo,
                              int page,
                              Action<IEnumerable<User>> callback,
                              Action<GitHubException> onError);
    }
}
