using System;
using System.Collections.Generic;
using System.Linq;
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
            var request = new RestRequest(resource, Method.GET);
            _gitHubClient.CallApiAsync<UserResult>(request,
                                                   API.v2,
                                                   u => callback(u.User),
                                                   onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback, Action<APICallError> onError) {
            _gitHubClient.CallApiAsync<UserResult>(new RestRequest("/user/show/", Method.GET),
                                                   API.v2,
                                                   u => callback(u.User),
                                                   onError);
        }

        public void IsFollowingAsync(string user,
                                     Action<bool> callback,
                                     Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            // API v3 has a dedicated resource for this functionality. For now
            // we can just do a bit more work with API v2 methods...
            GetAuthenticatedUserAsync(
                authenticated => {
                    GetFollowersAsync(user,
                                      f => callback(f.Where(u => u.Login == authenticated.Login).Count() > 0),
                                      onError);
                },
                onError);
        }

        public void FollowAsync(string user,
                                Action callback,
                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/follow/{0}", user);
            var request = new RestRequest(resource, Method.POST);
            _gitHubClient.CallApiAsync<object>(request,
                                               API.v2,
                                               s => callback(),
                                               onError);
        }

        public void UnfollowAsync(string user,
                                  Action callback,
                                  Action<APICallError> onError) {
            var resource = string.Format("/user/unfollow/{0}", user);
            var request = new RestRequest(resource, Method.POST);
            _gitHubClient.CallApiAsync<object>(request,
                                               API.v2,
                                               s => callback(),
                                               onError);
        }

        public void GetFollowersAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/followers?full=1", user);
            var request = new RestRequest(resource, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    API.v2,
                                                    u => callback(u.Users),
                                                    onError);
        }

        public void GetFollowingAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/following?full=1", user);
            var request = new RestRequest(resource, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    API.v2,
                                                    u => callback(u.Users),
                                                    onError);
        }

        public void SearchAsync(string query,
                                Action<IEnumerable<User>> callback,
                                Action<APICallError> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/user/search/{0}", query.Replace(' ', '+'));
            var request = new RestRequest(resource, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    API.v2,
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
            var request = new RestRequest(resource, Method.GET);
            _gitHubClient.CallApiAsync<RepositoriesResult>(request,
                                                           API.v2,
                                                           r => callback(r.Repositories),
                                                           onError);
        }
    }
}
