using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub.Services {
    public class UserService : IUserService {
        private readonly IGitHubClient _gitHubClient;

        public UserService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _gitHubClient = gitHubClient;
        }

        public void GetUserAsync(string user,
                                 Action<User> callback,
                                 Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}", user);
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            _gitHubClient.CallApiAsync<User>(request,
                                             r => callback(r.Data),
                                             onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback,
                                              Action<GitHubException> onError) {
            var request = new GitHubRequest("/user", API.v3, Method.GET);
            _gitHubClient.CallApiAsync<User>(request,
                                             r => callback(r.Data),
                                             onError);
        }

        public void IsFollowingAsync(string user,
                                     Action<bool> callback,
                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/following/{0}", user);
            var request = new GitHubRequest(resource, API.v3, Method.GET);

            _gitHubClient.CallApiAsync<object>(request,
                                         r => {
                                             Debug.Assert(false, "all responses should be errors");
                                             callback(true);
                                         },
                                         e => {
                                             if (e.Response.StatusCode == HttpStatusCode.NoContent) {
                                                 callback(true);
                                                 return;
                                             }

                                             if (e.Response.StatusCode == HttpStatusCode.NotFound) {
                                                 callback(false);
                                                 return;
                                             }

                                             onError(e);
                                         });
        }

        public void FollowAsync(string user,
                                Action callback,
                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/following/{0}", user);
            var request = new GitHubRequest(resource, API.v3, Method.PUT);
            _gitHubClient.CallApiAsync<object>(request,
                                               s => callback(),
                                               onError);
        }

        public void UnfollowAsync(string user,
                                  Action callback,
                                  Action<GitHubException> onError) {
            var resource = string.Format("/user/unfollowing/{0}", user);
            var request = new GitHubRequest(resource, API.v3, Method.DELETE);
            _gitHubClient.CallApiAsync<object>(request,
                                               s => callback(),
                                               onError);
        }

        public void GetFollowersAsync(string user,
                                      int page,
                                      Action<IEnumerable<User>> callback,
                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/followers", user);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            _gitHubClient.CallApiAsync<List<User>>(request,
                                                   r => callback(r.Data),
                                                   onError);
        }

        public void GetFollowingAsync(string user,
                                      int page,
                                      Action<IEnumerable<User>> callback,
                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/following", user);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            _gitHubClient.CallApiAsync<List<User>>(request,
                                                   r => callback(r.Data),
                                                   onError);
        }

        public void GetWatchersAsync(string user,
                                     string repo,
                                     int page,
                                     Action<IEnumerable<User>> callback,
                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/{0}/{1}/watchers",
                                         user,
                                         repo,
                                         Parameter.Page(page));
            var request = new GitHubRequest(resource, API.v3, Method.GET);
            _gitHubClient.CallApiAsync<List<User>>(request,
                                                       r => callback(r.Data),
                                                       onError);
        }
    }
}
