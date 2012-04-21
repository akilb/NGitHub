using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;
using NGitHub.Models.Dto;

namespace NGitHub.Services {
    public class PullRequestService : IPullRequestService {
        private readonly IGitHubClient _client;

        public PullRequestService(IGitHubClient client) {
            Requires.ArgumentNotNull(client, "client");

            _client = client;
        }

        public GitHubRequestAsyncHandle GetPullRequestAsync(string user,
                                                            string repo,
                                                            int pullRequestId,
                                                            Action<PullRequest> callback,
                                                            Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}", user, repo, pullRequestId);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            return _client.CallApiAsync<PullRequest>(request,
                                                     r => callback(r.Data),
                                                     onError);
        }

        public GitHubRequestAsyncHandle GetPullRequestsAsync(string user,
                                                             string repo,
                                                             State state,
                                                             int page,
                                                             Action<IEnumerable<PullRequest>> callback,
                                                             Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls", user, repo);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.State(state),
                                            Parameter.Page(page));

            return _client.CallApiAsync<List<PullRequest>>(request,
                                                           r => callback(r.Data),
                                                           onError);
        }

        public GitHubRequestAsyncHandle IsPullRequestMergedAsync(string user,
                                                                 string repo,
                                                                 int pullRequestId,
                                                                 Action<bool> callback,
                                                                 Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/merge",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            return _client.CallApiAsync<object>(
                                        request,
                                        r => {
                                            Debug.Assert(false, "all responses should be errors");
                                            callback(true);
                                        },
                                        e => {
                                            if (e.Response.StatusCode == HttpStatusCode.NoContent) {
                                                callback(true);
                                                return;
                                            }

                                            if (e.Response.StatusCode == HttpStatusCode.NotFound) {
                                                callback(false);
                                                return;
                                            }

                                            onError(e);
                                        });
        }

        public GitHubRequestAsyncHandle GetCommitsAsync(string user,
                                                        string repo,
                                                        int pullRequestId,
                                                        int page,
                                                        Action<IEnumerable<Commit>> callback,
                                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/commits",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource, API.v3, Method.GET, Parameter.Page(page));

            return _client.CallApiAsync<List<Commit>>(request,
                                                      r => callback(r.Data),
                                                      onError);
        }

        public GitHubRequestAsyncHandle GetFilesAsync(string user,
                                                      string repo,
                                                      int pullRequestId,
                                                      int page,
                                                      Action<IEnumerable<FileChanges>> callback,
                                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/files",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource, API.v3, Method.GET, Parameter.Page(page));

            return _client.CallApiAsync<List<FileChanges>>(request,
                                                           r => callback(r.Data),
                                                           onError);
        }

        public GitHubRequestAsyncHandle GetCommentAsync(string user,
                                                        string repo,
                                                        int commentId,
                                                        Action<CommitComment> callback,
                                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/comments/{2}",
                                         user,
                                         repo,
                                         commentId);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            return _client.CallApiAsync<CommitComment>(request,
                                                       r => callback(r.Data),
                                                       onError);
        }

        public GitHubRequestAsyncHandle GetCommentsAsync(string user,
                                                         string repo,
                                                         int pullRequestId,
                                                         int page,
                                                         Action<IEnumerable<CommitComment>> callback,
                                                         Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/comments",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource, API.v3, Method.GET, Parameter.Page(page));

            return _client.CallApiAsync<List<CommitComment>>(request,
                                                             r => callback(r.Data),
                                                             onError);
        }

        public GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                           string repo,
                                                           int pullRequestId,
                                                           string commitId,
                                                           string body,
                                                           string path,
                                                           int position,
                                                           Action<CommitComment> callback,
                                                           Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(body, "body");
            Requires.ArgumentNotNull(path, "path");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/comments",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.POST,
                                            new CommitCommentDto {
                                                CommitId = commitId,
                                                Body = body,
                                                Path = path,
                                                Position = position
                                            });
            return _client.CallApiAsync<CommitComment>(request,
                                                       r => callback(r.Data),
                                                       onError);
        }

        public GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                           string repo,
                                                           int pullRequestId,
                                                           int commentIdToReplyTo,
                                                           string body,
                                                           Action<CommitComment> callback,
                                                           Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");
            Requires.ArgumentNotNull(body, "body");

            var resource = string.Format("/repos/{0}/{1}/pulls/{2}/comments",
                                         user,
                                         repo,
                                         pullRequestId);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.POST,
                                            new ReplyCommentDto {
                                                Body = body,
                                                InReplyTo = commentIdToReplyTo
                                            });
            return _client.CallApiAsync<CommitComment>(request,
                                                       r => callback(r.Data),
                                                       onError);
        }
    }
}
