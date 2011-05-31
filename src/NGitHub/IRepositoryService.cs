using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IRepositoryService {
        void GetRepositoryAsync(string user,
                                string repo,
                                Action<Repository> callback,
                                Action<APICallError> onError);
        void GetWatchersAsync(string user,
                              string repo,
                              Action<IEnumerable<User>> callback,
                              Action<APICallError> onError);
        void GetBranchesAsync(string user,
                              string repo,
                              Action<IEnumerable<Branch>> callback,
                              Action<APICallError> onError);
        void GetForksAsync(string user,
                           string repo,
                           Action<IEnumerable<Repository>> callback,
                           Action<APICallError> onError);
        void SearchAsync(string query,
                         Action<IEnumerable<Repository>> callback,
                         Action<APICallError> onError);
    }
}
