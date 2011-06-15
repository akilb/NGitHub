using System;
using System.Collections.Generic;
using System.Linq;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class RepositoryService : IRepositoryService {
        private readonly IGitHubClient _client;

        public RepositoryService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetRepositoryAsync(string user,
                                       string repo,
                                       Action<Repository> callback,
                                       Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}", user, repo);
            var request = new RestRequest(resource, Method.GET);
            _client.CallApiAsync<RepositoryResult>(request,
                                                   API.v2,
                                                   r => callback(r.Repository),
                                                   onError);
        }

        public void GetWatchersAsync(string user,
                                     string repo,
                                     Action<IEnumerable<User>> callback,
                                     Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/watchers?full=1", user, repo);
            var request = new RestRequest(resource, Method.GET);
            _client.CallApiAsync<WatchersResult>(request,
                                                 API.v2,
                                                 w => callback(w.Watchers),
                                                 onError);
        }

        public void GetBranchesAsync(string user,
                                     string repo,
                                     Action<IEnumerable<Branch>> callback,
                                     Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/branches", user, repo);
            var request = new RestRequest(resource, Method.GET);
            _client.CallApiAsync<BranchesResult>(request,
                                                 API.v2,
                                                 b => {
                                                     var branches = new List<Branch>();
                                                     foreach (var pair in b.Branches) {
                                                         branches.Add(new Branch {
                                                             Name = pair.Key,
                                                             Hash = pair.Value
                                                         });
                                                     }

                                                     callback(branches);
                                                 },
                                                 onError);
        }

        public void GetForksAsync(string user,
                                  string repo,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/network", user, repo);
            var request = new RestRequest(resource);
            _client.CallApiAsync<NetworkResult>(request,
                                                API.v2,
                                                r => callback(r.Forks),
                                                onError);
        }

        public void SearchAsync(string query,
                                Action<IEnumerable<Repository>> callback,
                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/repos/search/{0}", query.Replace(' ', '+'));
            var request = new RestRequest(resource, Method.GET);
            _client.CallApiAsync<RepositoriesResult>(request,
                                                     API.v2,
                                                     r => callback(r.Repositories),
                                                     onError);
        }

        public void ForkAsync(string user,
                              string repo,
                              Action<Repository> callback,
                              Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/fork/{0}/{1}", user, repo);
            var request = new RestRequest(resource, Method.POST);
            _client.CallApiAsync<RepositoryResult>(request,
                                                   API.v2,
                                                   r => callback(r.Repository),
                                                   onError);
        }

        public void WatchAsync(string user,
                               string repo,
                               Action callback,
                               Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/watch/{0}/{1}", user, repo);
            var request = new RestRequest(resource, Method.POST);
            _client.CallApiAsync<object>(request,
                                         API.v2,
                                         o => callback(),
                                         onError);
        }

        public void UnwatchAsync(string user,
                            string repo,
                            Action callback,
                            Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("repos/unwatch/{0}/{1}", user, repo);
            var request = new RestRequest(resource, Method.POST);
            _client.CallApiAsync<object>(request,
                                         API.v2,
                                         o => callback(),
                                         onError);
        }

        public void IsWatchingAsync(string user,
                                    string repo,
                                    Action<bool> callback,
                                    Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");


            // Hopefully API v3 will have dedicated resource for this functionality.
            // For now we can just do a bit more work with API v2 methods...
            _client.Users.GetAuthenticatedUserAsync(
                authenticated => {
                    GetWatchersAsync(user,
                                     repo,
                                     w => callback(w.Where(u => u.Login == authenticated.Login).Count() > 0),
                                     onError);
                },
                onError);
        }
    }
}
