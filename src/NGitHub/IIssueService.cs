using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IIssueService {
        void GetRepositoryIssuesAsync(string user,
                                      string repo,
                                      int page,
                                      State state,
                                      Action<IEnumerable<Issue>> callback,
                                      Action<APICallError> onError);
        void GetRepositoryIssuesAsync(string user,
                                      string repo,
                                      int page,
                                      State state,
                                      SortBy sort,
                                      OrderBy direction,
                                      Action<IEnumerable<Issue>> callback,
                                      Action<APICallError> onError);
    }
}
