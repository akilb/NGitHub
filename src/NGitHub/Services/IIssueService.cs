using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IIssueService {
        GitHubRequestAsyncHandle CreateIssueAsync(string user,
                                                  string repo,
                                                  string title,
                                                  string body,
                                                  string assignee,
                                                  string mileStone,
                                                  string[] labels,
                                                  Action<Issue> callback,
                                                  Action<GitHubException> onError);

        GitHubRequestAsyncHandle GetIssueAsync(string user,
                                               string repo,
                                               int issueNumber,
                                               Action<Issue> callback,
                                               Action<GitHubException> onError);
        GitHubRequestAsyncHandle GetIssuesAsync(string user,
                                                string repo,
                                                State state,
                                                int page,
                                                Action<IEnumerable<Issue>> callback,
                                                Action<GitHubException> onError);
        GitHubRequestAsyncHandle CreateCommentAsync(string user,
                                                    string repo,
                                                    int issueNumber,
                                                    string comment,
                                                    Action<Comment> callback,
                                                    Action<GitHubException> onError);
        GitHubRequestAsyncHandle GetCommentsAsync(string user,
                                                  string repo,
                                                  int issueNumber,
                                                  int page,
                                                  Action<IEnumerable<Comment>> callback,
                                                  Action<GitHubException> onError);
    }
}
