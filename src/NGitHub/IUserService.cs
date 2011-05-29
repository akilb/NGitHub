using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IUserService {
        void GetUserAsync(string user, Action<User> callback, Action<APICallError> onError);
        void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError);

        void GetUserFollowersAsync(string user,
                                   int page,
                                   Action<IEnumerable<User>> callback,
                                   Action<APICallError> onError);
        void GetUserFollowersAsync(string user,
                                   int page,
                                   SortBy sort,
                                   OrderBy direction,
                                   Action<IEnumerable<User>> callback,
                                   Action<APICallError> onError);
        void GetUserFollowingAsync(string user,
                                   int page,
                                   Action<IEnumerable<User>> callback,
                                   Action<APICallError> onError);
        void GetUserFollowingAsync(string user,
                                   int page,
                                   SortBy sort,
                                   OrderBy direction,
                                   Action<IEnumerable<User>> callback,
                                   Action<APICallError> onError);
    }
}
