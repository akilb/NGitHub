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

            var resource = string.Format("/users/{0}", user);
            _gitHubClient.CallApiAsync<User>(resource, API.v3, Method.GET, callback, onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<User>("/user", API.v3, Method.GET, callback, onError);
        }

        public void GetFollowersAsync(string user,
                                      int page,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            GetFollowersAsync(user,
                              page,
                              Constants.DefaultSortBy,
                              Constants.DefaultOrderBy,
                              callback,
                              onError);
        }

        public void GetFollowersAsync(string user,
                                      int page,
                                      SortBy sort,
                                      OrderBy direction,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/followers?{1}",
                                         user,
                                         ApiHelpers.GetParametersString(page, sort, direction));
            _gitHubClient.CallApiAsync<List<User>>(resource,
                                                   API.v3,
                                                   Method.GET,
                                                   users => callback(users),
                                                   onError);
        }

        public void GetFollowingAsync(string user,
                                      int page,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            GetFollowingAsync(user,
                              page,
                              Constants.DefaultSortBy,
                              Constants.DefaultOrderBy,
                              callback,
                              onError);
        }

        public void GetFollowingAsync(string user,
                                          int page,
                                          SortBy sort,
                                          OrderBy direction,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/following?{1}",
                                         user,
                                         ApiHelpers.GetParametersString(page, sort, direction));
            _gitHubClient.CallApiAsync<List<User>>(resource,
                                                   API.v3,
                                                   Method.GET,
                                                   users => callback(users),
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
