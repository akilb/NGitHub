using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub.Services {
    public interface IPullRequestService {
        GitHubRequestAsyncHandle GetPullRequestAsync(string user,
                                                     string repo,
                                                     int pullRequestId,
                                                     Action<PullRequest> callback,
                                                     Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetPullRequestsAsync(string user,
                                                      string repo,
                                                      State state,
                                                      int page,
                                                      Action<IEnumerable<PullRequest>> callback,
                                                      Action<GitHubException> onError);

        GitHubRequestAsyncHandle IsPullRequestMergedAsync(string user,
                                                          string repo,
                                                          int pullRequestId,
                                                          Action<bool> callback,
                                                          Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetCommitsAsync(string user,
                                                 string repo,
                                                 int pullRequestId,
                                                 int page,
                                                 Action<IEnumerable<Commit>> callback,
                                                 Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetFilesAsync(string user,
                                               string repo,
                                               int pullRequestId,
                                               int page,
                                               Action<IEnumerable<FileChanges>> callback,
                                               Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetCommentAsync(string user,
                                                 string repo,
                                                 int commentId,
                                                 Action<CommitComment> callback,
                                                 Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetCommentsAsync(string user,
                                                  string repo,
                                                  int pullRequestId,
                                                  int page,
                                                  Action<IEnumerable<CommitComment>> callback,
                                                  Action<GitHubException> onError);

        GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                    string repo,
                                                    int pullRequestId,
                                                    string commitId,
                                                    string body,
                                                    string path,
                                                    string position,
                                                    Action<CommitComment> callback,
                                                    Action<GitHubException> onError);

        GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                    string repo,
                                                    int pullRequestId,
                                                    int commentIdToReplyTo,
                                                    string body,
                                                    Action<CommitComment> callback,
                                                    Action<GitHubException> onError);
    }
}
