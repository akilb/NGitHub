using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IUserService {
        void GetUserAsync(string user, Action<User> callback, Action<APICallError> onError);
        void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError);

        void IsFollowingAsync(string user, Action<bool> callback, Action<APICallError> onError);
        void FollowAsync(string user, Action callback, Action<APICallError> onError);
        void UnfollowAsync(string user, Action callback, Action<APICallError> onError);

        void GetFollowersAsync(string user,
                               Action<IEnumerable<User>> callback,
                               Action<APICallError> onError);
        void GetFollowingAsync(string user,
                               Action<IEnumerable<User>> callback,
                               Action<APICallError> onError);

        void GetRepositoriesAsync(string user,
                                  Action<IEnumerable<Repository>> callback,
                                  Action<APICallError> onError);
        void GetWatchedRepositoriesAsync(string user,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<APICallError> onError);

        void SearchAsync(string query,
                         Action<IEnumerable<User>> callback,
                         Action<APICallError> onError);
    }
}
