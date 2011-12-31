using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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

        public void GetRepositoryAsync(string user,
                                       string repo,
                                       Action<Repository> callback,
                                       Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            _client.CallApiAsync<Repository>(request,
                                             r => callback(r.Data),
                                             onError);
        }

        public void GetRepositoriesAsync(string user,
                                         int page,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/repos", user);
            GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public void GetWatchedRepositoriesAsync(string user,
                                                int page,
                                                Action<IEnumerable<Repository>> callback,
                                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/watched", user);
            GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public void ForkAsync(string user,
                              string repo,
                              Action<Repository> callback,
                              Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            // TODO: Allow for forking into an Org...
            var resource = string.Format("/repos/{0}/{1}/forks", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.POST);
            _client.CallApiAsync<Repository>(request,
                                             r => callback(r.Data),
                                             onError);
        }

        public void GetForksAsync(string user,
                                  string repo,
                                  int page,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/forks", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public void WatchAsync(string user,
                               string repo,
                               Action callback,
                               Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.PUT);
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

            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.DELETE);
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


            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            _client.CallApiAsync<object>(request,
                                         r => {
                                             Debug.Assert(false, "all responses should be errors");
                                             callback(true);
                                         },
                                         e => {
                                             if (e.Response.StatusCode == HttpStatusCode.NoContent) {
                                                 callback(true);
                                                 return;
                                             }

                                             if (e.Response.StatusCode == HttpStatusCode.NotFound) {
                                                 callback(false);
                                                 return;
                                             }

                                             onError(e);
                                         });
        }

        public void GetBranchesAsync(string user,
                                     string repo,
                                     int page,
                                     Action<IEnumerable<Branch>> callback,
                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/branches", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            _client.CallApiAsync<List<Branch>>(request,
                                               r => callback(r.Data),
                                               onError);
        }

        private void GetRepositoriesAsyncInternal(string resource,
                                                  int page,
                                                  Action<IEnumerable<Repository>> callback,
                                                  Action<GitHubException> onError) {
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            _client.CallApiAsync<List<Repository>>(request,
                                                   r => callback(r.Data),
                                                   onError);
        }
    }
}
