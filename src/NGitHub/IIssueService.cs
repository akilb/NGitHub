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
                            int page,
                            State state,
                            Action<IEnumerable<Issue>> callback,
                            Action<APICallError> onError);
        void GetIssuesAsync(string user,
                            string repo,
                            int page,
                            State state,
                            SortBy sort,
                            OrderBy direction,
                            Action<IEnumerable<Issue>> callback,
                            Action<APICallError> onError);
    }
}
