using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    class RepositoryService : IRepositoryService {
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
            _client.CallApiAsync<RepositoryResult>(resource,
                                                   API.v2,
                                                   Method.GET,
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
            _client.CallApiAsync<List<User>>(resource,
                                             API.v2,
                                             Method.GET,
                                             watchers => callback(watchers),
                                             onError);
        }

        public void GetBranchesAsync(string user,
                                     string repo,
                                     Action<IEnumerable<Branch>> callback,
                                     Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/branches", user, repo);
            _client.CallApiAsync<BranchesResult>(resource,
                                                 API.v2,
                                                 Method.GET,
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
            _client.CallApiAsync<NetworkResult>(resource,
                                                API.v2,
                                                Method.GET,
                                                r => callback(r.Forks),
                                                onError);
        }

        public void SearchAsync(string query,
                                Action<IEnumerable<Repository>> callback,
                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/repos/search/{0}", query.Replace(' ', '+'));
            _client.CallApiAsync<RepositoriesResult>(resource,
                                                     API.v2,
                                                     Method.GET,
                                                     r => callback(r.Repositories),
                                                     onError);
        }
    }
}
