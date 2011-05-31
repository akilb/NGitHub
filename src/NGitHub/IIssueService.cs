using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IIssueService {
        void GetIssueAsync(string user,
                           string repo,
                           string issueId,
                           Action<Issue> callback,
                           Action<APICallError> onError);
        void GetIssuesAsync(string user,
                            string repo,
                            State state,
                            Action<IEnumerable<Issue>> callback,
                            Action<APICallError> onError);
        void GetCommentsAsync(string user,
                              string repo,
                              int issueNumber,
                              Action<IEnumerable<Comment>> callback,
                              Action<APICallError> onError);
    }
}
