using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface ICommentService {
        void GetIssueCommentsAsync(string user,
                                   string repo,
                                   int issueNumber,
                                   int page,
                                   Action<IEnumerable<Comment>> callback,
                                   Action<APICallError> onError);
        void GetIssueCommentsAsync(string user,
                                   string repo,
                                   int issueNumber,
                                   int page,
                                   SortBy sort,
                                   OrderBy direction,
                                   Action<IEnumerable<Comment>> callback,
                                   Action<APICallError> onError);

        void GetGistCommentsAsync(string gistId,
                                  int page,
                                  Action<IEnumerable<Comment>> callback,
                                  Action<APICallError> onError);
        void GetGistCommentsAsync(string gistId,
                                  int page,
                                  SortBy sort,
                                  OrderBy direction,
                                  Action<IEnumerable<Comment>> callback,
                                  Action<APICallError> onError);
    }
}
