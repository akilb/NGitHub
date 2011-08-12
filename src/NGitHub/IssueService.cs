using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;

namespace NGitHub {
    public class IssueService : IIssueService {
        private IGitHubClient _client;

        public IssueService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetIssueAsync(string user,
                                  string repo,
                                  string issueId,
                                  Action<Issue> callback,
                                  Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(issueId, "issueId");

            var resource = string.Format("/issues/show/{0}/{1}/{2}", user, repo, issueId);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<IssueResult>(request, i => callback(i.Issue), onError);
        }

        public void GetIssuesAsync(string user,
                                   string repo,
                                   State state,
                                   Action<IEnumerable<Issue>> callback,
                                   Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/issues/list/{0}/{1}/{2}", user, repo, state.GetText());
            var request = new GitHubRequest(resource, API.v2, Method.GET);

            _client.CallApiAsync<IssuesResult>(request,
                                               i => callback(i.Issues),
                                               onError);
        }

        public void CreateCommentAsync(string user,
                                       string repo,
                                       int issueNumber,
                                       string comment,
                                       Action<Comment> callback,
                                       Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(comment, "comment");

            var resource = string.Format("/issues/comment/{0}/{1}/{2}",
                                         user,
                                         repo,
                                         issueNumber);
            var request = new GitHubRequest(resource, API.v2, Method.POST, new Parameter("comment", comment));

            _client.CallApiAsync<CommentResult>(request, c => callback(c.Comment), onError);
        }

        public void GetCommentsAsync(string user,
                                     string repo,
                                     int issueNumber,
                                     Action<IEnumerable<Comment>> callback,
                                     Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/issues/comments/{0}/{1}/{2}",
                                         user,
                                         repo,
                                         issueNumber);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<CommentsResult>(request,
                                                 c => callback(c.Comments),
                                                 onError);
        }
    }
}
