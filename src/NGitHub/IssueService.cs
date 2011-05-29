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

            var resource = string.Format("/repos/{0}/{1}/issues/{2}", user, repo, issueId);
            _client.CallApiAsync<Issue>(resource, API.Version3, Method.GET, callback, onError);
        }

        public void GetIssuesAsync(string user,
                                   string repo,
                                   int page,
                                   State state,
                                   Action<IEnumerable<Models.Issue>> callback,
                                   Action<APICallError> onError) {
            GetIssuesAsync(user,
                           repo,
                           page,
                           state,
                           SortBy.Created,
                           OrderBy.Descending,
                           callback,
                           onError);
        }

        public void GetIssuesAsync(string user,
                                   string repo,
                                   int page,
                                   State state,
                                   SortBy sort,
                                   OrderBy direction,
                                   Action<IEnumerable<Models.Issue>> callback,
                                   Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.IsTrue(page > 0, "page");

            var resource
                = string.Format("/repos/{0}/{1}/issues?page={1}&state={2}&sort={3}&direction={4}",
                                user,
                                repo,
                                page,
                                state.GetText(),
                                sort.GetText(),
                                direction.GetText());

            _client.CallApiAsync<List<Issue>>(resource,
                                              API.Version3,
                                              Method.GET,
                                              issues => callback(issues),
                                              onError);
        }
    }
}
