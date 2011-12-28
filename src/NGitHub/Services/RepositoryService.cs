using System;
using System.Collections.Generic;
using System.Linq;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub.Services {
    public class RepositoryService : IRepositoryService {
        private readonly IGitHubClient _client;

        public RepositoryService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void SearchAsync(string query,
                                Action<IEnumerable<Repository>> callback,
                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/repos/search/{0}", query.Replace(' ', '+'));
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<RepositoriesResult>(request,
                                                     r => callback(r.Data.Repositories),
                                                     onError);
        }

        public void GetRepositoryAsync(string user,
                                       string repo,
                                       Action<Repository> callback,
                                       Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<RepositoryResult>(request,
                                                   r => callback(r.Data.Repository),
                                                   onError);
        }

        public void GetRepositoriesAsync(string user,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/repos/show/{0}", user);
            GetRepositoriesAsyncInternal(resource, callback, onError);
        }

        public void GetWatchedRepositoriesAsync(string user,
                                                Action<IEnumerable<Repository>> callback,
                                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/repos/watched/{0}", user);
            GetRepositoriesAsyncInternal(resource, callback, onError);
        }

        public void ForkAsync(string user,
                              string repo,
                              Action<Repository> callback,
                              Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/fork/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.POST);
            _client.CallApiAsync<RepositoryResult>(request,
                                                   r => callback(r.Data.Repository),
                                                   onError);
        }

        public void GetForksAsync(string user,
                                  string repo,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/network", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<NetworkResult>(request,
                                                r => callback(r.Data.Forks),
                                                onError);
        }

        public void WatchAsync(string user,
                               string repo,
                               Action callback,
                               Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/watch/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.POST);
            _client.CallApiAsync<object>(request,
                                         r => callback(),
                                         onError);
        }

        public void UnwatchAsync(string user,
                                 string repo,
                                 Action callback,
                                 Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/unwatch/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.POST);
            _client.CallApiAsync<object>(request,
                                         r => callback(),
                                         onError);
        }

        public void IsWatchingAsync(string user,
                                    string repo,
                                    Action<bool> callback,
                                    Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");


            // Hopefully API v3 will have dedicated resource for this functionality.
            // For now we can just do a bit more work with API v2 methods...
            _client.Users.GetAuthenticatedUserAsync(
                authenticated => {
                    _client.Users.GetWatchersAsync(
                                    user,
                                    repo,
                                    w => callback(w.Where(u => u.Login == authenticated.Login).Count() > 0),
                                    onError);
                },
                onError);
        }

        public void GetBranchesAsync(string user,
                                     string repo,
                                     Action<IEnumerable<Branch>> callback,
                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/branches", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<BranchesResult>(request,
                                                 r => {
                                                     var branches = new List<Branch>();
                                                     foreach (var pair in r.Data.Branches) {
                                                         branches.Add(new Branch {
                                                             Name = pair.Key,
                                                             Hash = pair.Value
                                                         });
                                                     }

                                                     callback(branches);
                                                 },
                                                 onError);
        }

        private void GetRepositoriesAsyncInternal(string resource,
                                                  Action<IEnumerable<Repository>> callback,
                                                  Action<GitHubException> onError) {
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<RepositoriesResult>(request,
                                                     r => callback(r.Data.Repositories),
                                                     onError);
        }
    }
}
