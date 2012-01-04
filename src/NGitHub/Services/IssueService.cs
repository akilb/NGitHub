using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub.Services {
    public class IssueService : IIssueService {
        private IGitHubClient _client;

        public IssueService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public GitHubRequestAsyncHandle GetIssueAsync(string user,
                                                      string repo,
                                                      int issueNumber,
                                                      Action<Issue> callback,
                                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/issues/{2}", user, repo, issueNumber);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            return _client.CallApiAsync<Issue>(request,
                                               r => callback(r.Data),
                                               onError);
        }

        public GitHubRequestAsyncHandle GetIssuesAsync(string user,
                                                       string repo,
                                                       State state,
                                                       int page,
                                                       Action<IEnumerable<Issue>> callback,
                                                       Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/issues", user, repo);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.State(state),
                                            Parameter.Page(page));

            return _client.CallApiAsync<List<Issue>>(request,
                                                     r => callback(r.Data),
                                                     onError);
        }

        public GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                           string repo,
                                                           int issueNumber,
                                                           string comment,
                                                           Action<Comment> callback,
                                                           Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(comment, "comment");

            var resource = string.Format("/repos/{0}/{1}/issues/{2}/comments",
                                         user,
                                         repo,
                                         issueNumber);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.POST,
                                            Parameter.Comment(comment));

            return _client.CallApiAsync<Comment>(request,
                                                 r => callback(r.Data),
                                                 onError);
        }

        public GitHubRequestAsyncHandle GetCommentsAsync(string user,
                                                         string repo,
                                                         int issueNumber,
                                                         int page,
                                                         Action<IEnumerable<Comment>> callback,
                                                         Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/issues/{2}/comments",
                                         user,
                                         repo,
                                         issueNumber);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            return _client.CallApiAsync<List<Comment>>(request,
                                                       r => callback(r.Data),
                                                       onError);
        }
    }
}
