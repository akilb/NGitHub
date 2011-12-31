using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IRepositoryService {
        void GetRepositoryAsync(string user,
                                string repo,
                                Action<Repository> callback,
                                Action<GitHubException> onError);

        void GetRepositoriesAsync(string user,
                                  int page,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<GitHubException> onError);

        void ForkAsync(string user,
                       string repo,
                       Action<Repository> callback,
                       Action<GitHubException> onError);

        void GetForksAsync(string user,
                           string repo,
                           int page,
                           Action<IEnumerable<Repository>> callback,
                           Action<GitHubException> onError);

        void WatchAsync(string user,
                        string repo,
                        Action callback,
                        Action<GitHubException> onError);

        void UnwatchAsync(string user,
                          string repo,
                          Action callback,
                          Action<GitHubException> onError);

        void GetWatchedRepositoriesAsync(string user,
                                         int page,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<GitHubException> onError);

        void IsWatchingAsync(string user,
                             string repo,
                             Action<bool> callback,
                             Action<GitHubException> onError);

        void GetBranchesAsync(string user,
                              string repo,
                              int page,
                              Action<IEnumerable<Branch>> callback,
                              Action<GitHubException> onError);
    }
}
