using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IRepositoryService {
        GitHubRequestAsyncHandle GetRepositoryAsync(string user,
                                                    string repo,
                                                    Action<Repository> callback,
                                                    Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetRepositoriesAsync(string user,
                                                      int page,
                                                      Action<IEnumerable<Repository>> callback,
                                                      Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetRepositoriesAsync(string user,
                                                      int page,
                                                      int perPage,
                                                      Action<IEnumerable<Repository>> callback,
                                                      Action<GitHubException> onError);

        GitHubRequestAsyncHandle ForkAsync(string user,
                                           string repo,
                                           Action<Repository> callback,
                                           Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetForksAsync(string user,
                                               string repo,
                                               int page,
                                               Action<IEnumerable<Repository>> callback,
                                               Action<GitHubException> onError);

        GitHubRequestAsyncHandle WatchAsync(string user,
                                            string repo,
                                            Action callback,
                                            Action<GitHubException> onError);

        GitHubRequestAsyncHandle UnwatchAsync(string user,
                                              string repo,
                                              Action callback,
                                              Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetWatchedRepositoriesAsync(string user,
                                                             int page,
                                                             Action<IEnumerable<Repository>> callback,
                                                             Action<GitHubException> onError);

        GitHubRequestAsyncHandle IsWatchingAsync(string user,
                                                 string repo,
                                                 Action<bool> callback,
                                                 Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetBranchesAsync(string user,
                                                  string repo,
                                                  int page,
                                                  Action<IEnumerable<Branch>> callback,
                                                  Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetCommitAsync(string user,
                                                string repo,
                                                string sha,
                                                Action<Commit> callback,
                                                Action<GitHubException> onError);
        GitHubRequestAsyncHandle GetCommitsAsync(string user,
                                                 string repo,
                                                 string branch,
                                                 string lastSha,
                                                 Action<IEnumerable<Commit>> callback,
                                                 Action<GitHubException> onError);
    }
}
