using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub.Services {
    public class CommitService : ICommitService {
        private readonly IGitHubClient _client;

        public CommitService(GitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetCommitAsync(string user,
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
            _client.CallApiAsync<Commit>(request,
                                         r => callback(r.Data),
                                         onError);
        }

        public void GetCommitsAsync(string user,
                                    string repo,
                                    string branch,
                                    int page,
                                    Action<IEnumerable<Commit>> callback,
                                    Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(branch, "branch");
            Requires.IsTrue(page > 0, "page");

            var resource = string.Format("/repos/{0}/{1}/commits", user, repo);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page),
                                            Parameter.Sha(branch));
            _client.CallApiAsync<List<Commit>>(request,
                                               r => callback(r.Data),
                                               onError);
        }
    }
}
