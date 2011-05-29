using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    class UserService : IUserService {
        private readonly IGitHubClient _gitHubClient;

        public UserService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _gitHubClient = gitHubClient;
        }

        public void GetUserAsync(string user, Action<User> callback, Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}", user);
            _gitHubClient.CallApiAsync<User>(resource, API.Version3, Method.GET, callback, onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<User>("/user", API.Version3, Method.GET, callback, onError);
        }

        public void GetUserFollowersAsync(string user,
                                          int page,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            GetUserFollowersAsync(user,
                                  page,
                                  Constants.DefaultSortBy,
                                  Constants.DefaultOrderBy,
                                  callback,
                                  onError);
        }

        public void GetUserFollowersAsync(string user,
                                          int page,
                                          SortBy sort,
                                          OrderBy direction,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/followers/?{1}",
                                         user,
                                         ApiHelpers.GetParametersString(page, sort, direction));
            GetUsersAsync(resource, API.Version3, callback, onError);
        }

        public void GetUserFollowingAsync(string user,
                                          int page,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            GetUserFollowingAsync(user,
                                  page,
                                  Constants.DefaultSortBy,
                                  Constants.DefaultOrderBy,
                                  callback,
                                  onError);
        }

        public void GetUserFollowingAsync(string user,
                                          int page,
                                          SortBy sort,
                                          OrderBy direction,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/following/?{1}",
                                         user,
                                         ApiHelpers.GetParametersString(page, sort, direction));
            GetUsersAsync(resource, API.Version3, callback, onError);
        }

        private void GetUsersAsync(string resource,
                                   API version,
                                   Action<IEnumerable<User>> callback,
                                   Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<List<User>>(resource,
                                                   version,
                                                   Method.GET,
                                                   users => callback(users),
                                                   onError);
        }
    }
}
