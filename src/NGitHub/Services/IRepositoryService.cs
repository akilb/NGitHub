using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IRepositoryService {
        void ForkAsync(string user,
                       string repo,
                       Action<Repository> callback,
                       Action<GitHubException> onError);
        void WatchAsync(string user,
                        string repo,
                        Action callback,
                        Action<GitHubException> onError);
        void UnwatchAsync(string user,
                          string repo,
                          Action callback,
                          Action<GitHubException> onError);
        void IsWatchingAsync(string user,
                             string repo,
                             Action<bool> callback,
                             Action<GitHubException> onError);

        void GetRepositoryAsync(string user,
                                string repo,
                                Action<Repository> callback,
                                Action<GitHubException> onError);
        void GetWatchersAsync(string user,
                              string repo,
                              Action<IEnumerable<User>> callback,
                              Action<GitHubException> onError);
        void GetBranchesAsync(string user,
                              string repo,
                              Action<IEnumerable<Branch>> callback,
                              Action<GitHubException> onError);
        void GetForksAsync(string user,
                           string repo,
                           Action<IEnumerable<Repository>> callback,
                           Action<GitHubException> onError);
        void SearchAsync(string query,
                         Action<IEnumerable<Repository>> callback,
                         Action<GitHubException> onError);
    }
}
