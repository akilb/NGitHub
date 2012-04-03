using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;
using Newtonsoft.Json;

namespace NGitHub.Services {
    public class RepositoryService : IRepositoryService {
        private readonly IGitHubClient _client;

        public RepositoryService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public GitHubRequestAsyncHandle GetRepositoryAsync(string user,
                                                           string repo,
                                                           Action<Repository> callback,
                                                           Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            return _client.CallApiAsync<Repository>(request,
                                                    r => callback(r.Data),
                                                    onError);
        }

        public GitHubRequestAsyncHandle GetRepositoriesAsync(string user,
                                                             int page,
                                                             Action<IEnumerable<Repository>> callback,
                                                             Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/repos", user);
            return GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public GitHubRequestAsyncHandle GetWatchedRepositoriesAsync(string user,
                                                                    int page,
                                                                    Action<IEnumerable<Repository>> callback,
                                                                    Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/watched", user);
            return GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public GitHubRequestAsyncHandle ForkAsync(string user,
                                                  string repo,
                                                  Action<Repository> callback,
                                                  Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            // TODO: Allow for forking into an Org...
            var resource = string.Format("/repos/{0}/{1}/forks", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.POST);
            return _client.CallApiAsync<Repository>(request,
                                                    r => callback(r.Data),
                                                    onError);
        }

        public GitHubRequestAsyncHandle GetForksAsync(string user,
                                                      string repo,
                                                      int page,
                                                      Action<IEnumerable<Repository>> callback,
                                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/forks", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            return GetRepositoriesAsyncInternal(resource, page, callback, onError);
        }

        public GitHubRequestAsyncHandle WatchAsync(string user,
                                                   string repo,
                                                   Action callback,
                                                   Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.PUT);
            return _client.CallApiAsync<object>(request,
                                                r => callback(),
                                                onError);
        }

        public GitHubRequestAsyncHandle UnwatchAsync(string user,
                                                     string repo,
                                                     Action callback,
                                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.DELETE);
            return _client.CallApiAsync<object>(request,
                                                r => callback(),
                                                onError);
        }

        public GitHubRequestAsyncHandle IsWatchingAsync(string user,
                                                        string repo,
                                                        Action<bool> callback,
                                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");


            var resource = string.Format("/user/watched/{0}/{1}", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            return _client.CallApiAsync<object>(
                                        request,
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

        public GitHubRequestAsyncHandle GetBranchesAsync(string user,
                                                         string repo,
                                                         int page,
                                                         Action<IEnumerable<Branch>> callback,
                                                         Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/branches", user, repo);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            return _client.CallApiAsync<List<Branch>>(request,
                                                      r => callback(r.Data),
                                                      onError);
        }

        public GitHubRequestAsyncHandle GetCommitAsync(string user,
                                                       string repo,
                                                       string sha,
                                                       Action<Commit> callback,
                                                       Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(sha, "sha");

            var resource = string.Format("/repos/{0}/{1}/commits/{2}", user, repo, sha);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET);
            return _client.CallApiAsync<Commit>(request,
                                                r => callback(r.Data),
                                                onError);
        }

        public GitHubRequestAsyncHandle GetCommitsAsync(string user,
                                                        string repo,
                                                        string branch,
                                                        string lastSha,
                                                        Action<IEnumerable<Commit>> callback,
                                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(branch, "branch");

            var resource = string.Format("/repos/{0}/{1}/commits", user, repo);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            new Parameter("last_sha", lastSha ?? string.Empty),
                                            new Parameter("top", branch));

            // We can't just deserialize every response as a List<Commit> because
            // repos that has no commits will return: 
            //  { "message": "The repository is empty." }
            //
            return _client.CallApiAsync(
                        request,
                        r => {
                            List<Commit> issues = null;
                            try {
                                issues = JsonConvert.DeserializeObject<List<Commit>>(r.Content);
                            }
                            catch {
                                onError(new GitHubException(r, ErrorType.Unknown));
                                return;
                            }

                            callback(issues);
                        },
                        e => {
                            if (e.Response.StatusCode == HttpStatusCode.Conflict) {
                                // This is the code that is returned when a repo is empty.
                                callback(new Commit[] { });
                                return;
                            }

                            onError(e);
                        });
        }

        private GitHubRequestAsyncHandle GetRepositoriesAsyncInternal(
                                            string resource,
                                            int page,
                                            Action<IEnumerable<Repository>> callback,
                                            Action<GitHubException> onError) {
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            return _client.CallApiAsync<List<Repository>>(request,
                                                          r => callback(r.Data),
                                                          onError);
        }
    }
}
