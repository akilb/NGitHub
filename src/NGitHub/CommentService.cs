using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class CommentService : ICommentService {
        private readonly IGitHubClient _client;

        public CommentService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetIssueCommentsAsync(string user,
                                          string repo,
                                          int issueNumber,
                                          int page,
                                          Action<IEnumerable<Comment>> callback,
                                          Action<APICallError> onError) {
            GetIssueCommentsAsync(user,
                                  repo,
                                  issueNumber,
                                  page,
                                  Constants.DefaultSortBy,
                                  Constants.DefaultOrderBy,
                                  callback,
                                  onError);
        }

        public void GetIssueCommentsAsync(string user,
                                          string repo,
                                          int issueNumber,
                                          int page,
                                          SortBy sort,
                                          OrderBy direction,
                                          Action<IEnumerable<Comment>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.IsTrue(page > 0, "page");

            var resource
                = string.Format("/repos/{0}/{1}/issues/{2}/comments?{3}",
                                user,
                                repo,
                                issueNumber,
                                ApiHelpers.GetParametersString(page, sort, direction));
            GetCommentsAsync(resource, callback, onError);
        }

        public void GetGistCommentsAsync(string gistId,
                                         int page,
                                         Action<IEnumerable<Comment>> callback,
                                         Action<APICallError> onError) {
            GetGistCommentsAsync(gistId,
                                 page,
                                 Constants.DefaultSortBy,
                                 Constants.DefaultOrderBy,
                                 callback,
                                 onError);
        }

        public void GetGistCommentsAsync(string gistId,
                                         int page,
                                         SortBy sort,
                                         OrderBy direction,
                                         Action<IEnumerable<Comment>> callback,
                                         Action<APICallError> onError) {
            Requires.ArgumentNotNull(gistId, "gistId");
            Requires.IsTrue(page > 0, "page");

            var resource = string.Format("/gists/{0}/comments?{1}",
                                         gistId,
                                         page,
                                         ApiHelpers.GetParametersString(page, sort, direction));
            GetCommentsAsync(resource, callback, onError);
        }

        private void GetCommentsAsync(string resource,
                                      Action<IEnumerable<Comment>> callback,
                                      Action<APICallError> onError) {
            _client.CallApiAsync<List<Comment>>(resource,
                                                API.Version3,
                                                Method.GET,
                                                commments => callback(commments),
                                                onError);
        }
    }
}
