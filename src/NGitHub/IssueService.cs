using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

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
            _client.CallApiAsync<IssueResult>(resource, API.v2, Method.GET, i => callback(i.Issue), onError);
        }

        public void GetIssuesAsync(string user,
                                   string repo,
                                   State state,
                                   Action<IEnumerable<Issue>> callback,
                                   Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/issues/list/{0}/{1}/{2}", user, repo, state.GetText());

            _client.CallApiAsync<IssuesResult>(resource,
                                               API.v2,
                                               Method.GET,
                                               i => callback(i.Issues),
                                               onError);
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
            _client.CallApiAsync<CommentsResult>(resource,
                                                 API.v2,
                                                 Method.GET,
                                                 c => callback(c.Comments),
                                                 onError);
        }
    }
}
