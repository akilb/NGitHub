using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class UserService : IUserService {
        private readonly IGitHubClient _gitHubClient;

        public UserService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _gitHubClient = gitHubClient;
        }

        public void GetUserAsync(string user, Action<User> callback, Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}", user);
            _gitHubClient.CallApiAsync<UserResult>(resource,
                                                   API.v2,
                                                   Method.GET,
                                                   u => callback(u.User),
                                                   onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<UserResult>("/user/show/",
                                                   API.v2,
                                                   Method.GET,
                                                   u => callback(u.User),
                                                   onError);
        }

        public void GetFollowersAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/followers?full=1", user);
            _gitHubClient.CallApiAsync<UsersResult>(resource,
                                                    API.v2,
                                                    Method.GET,
                                                    u => callback(u.Users),
                                                    onError);
        }

        public void GetFollowingAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/following?full=1", user);
            _gitHubClient.CallApiAsync<UsersResult>(resource,
                                                    API.v2,
                                                    Method.GET,
                                                    u => callback(u.Users),
                                                    onError);
        }

        public void SearchAsync(string query,
                                Action<IEnumerable<User>> callback,
                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/user/search/{0}", query.Replace(' ', '+'));
            _gitHubClient.CallApiAsync<UsersResult>(resource,
                                                    API.v2,
                                                    Method.GET,
                                                    u => callback(u.Users),
                                                    onError);
        }

        public void GetRepositoriesAsync(string user,
                                         Action<IEnumerable<Repository>> callback,
                                         Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/repos/show/{0}", user);
            GetRepositoriesAsyncInternal(resource, callback, onError);
        }

        public void GetWatchedRepositoriesAsync(string user,
                                                Action<IEnumerable<Repository>> callback,
                                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/repos/watched/{0}", user);
            GetRepositoriesAsyncInternal(resource, callback, onError);
        }

        private void GetRepositoriesAsyncInternal(string resource,
                                                  Action<IEnumerable<Repository>> callback,
                                                  Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<RepositoriesResult>(resource,
                                                           API.v2,
                                                           Method.GET,
                                                           r => callback(r.Repositories),
                                                           onError);
        }
    }
}
