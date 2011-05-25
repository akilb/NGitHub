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

        public void GetRepositoryIssuesAsync(string user,
                                             string repo,
                                             int page,
                                             State state,
                                             Action<IEnumerable<Models.Issue>> callback,
                                             Action<APICallError> onError) {
            GetRepositoryIssuesAsync(user,
                                     repo,
                                     page,
                                     state,
                                     SortBy.Created,
                                     OrderBy.Descending,
                                     callback,
                                     onError);
        }

        public void GetRepositoryIssuesAsync(string user,
                                             string repo,
                                             int page,
                                             State state,
                                             SortBy sort,
                                             OrderBy direction,
                                             Action<IEnumerable<Models.Issue>> callback,
                                             Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(onError, "onError");
            var resource
                = string.Format("/repos/{0}/{1}/issues?page={1}&state={2}&sort={3}&direction={4}",
                                user,
                                repo,
                                page,
                                state.GetText(),
                                sort.GetText(),
                                direction.GetText());

            _client.CallApiAsync<List<Issue>>(resource,
                                              Method.GET,
                                              issues => callback(issues),
                                              onError);
        }
    }
}
