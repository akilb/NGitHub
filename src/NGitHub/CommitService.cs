using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class CommitService : ICommitService {
        private readonly IGitHubClient _client;

        public CommitService(GitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetCommitsAsync(string user,
                                    string repo,
                                    string branch,
                                    int pageNo,
                                    Action<IEnumerable<Commit>> callback,
                                    Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(branch, "branch");
            Requires.IsTrue(pageNo > 0, "page");

            var resource = string.Format("/commits/list/{0}/{1}/{2}?page={3}",
                                         user,
                                         repo,
                                         branch,
                                         pageNo);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<CommitsResult>(request,
                                                r => callback(r.Data.Commits),
                                                onError);
        }
    }
}
