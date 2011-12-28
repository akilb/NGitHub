using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SearchAsync(string query,
                                Action<IEnumerable<User>> callback,
                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(query, "query");

            var resource = string.Format("/user/search/{0}", query.Replace(' ', '+'));
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    r => callback(r.Data.Users),
                                                    onError);
        }

        public void GetUserAsync(string user, Action<User> callback, Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}", user);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _gitHubClient.CallApiAsync<UserResult>(request,
                                                   r => callback(r.Data.User),
                                                   onError);
        }

        public void GetAuthenticatedUserAsync(Action<User> callback, Action<GitHubException> onError) {
            var request = new GitHubRequest("/user/show/", API.v2, Method.GET);
            _gitHubClient.CallApiAsync<UserResult>(request,
                                                   r => callback(r.Data.User),
                                                   onError);
        }

        public void IsFollowingAsync(string user,
                                     Action<bool> callback,
                                     Action<GitHubException> onError) {
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
                                Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/follow/{0}", user);
            var request = new GitHubRequest(resource, API.v2, Method.POST);
            _gitHubClient.CallApiAsync<object>(request,
                                               s => callback(),
                                               onError);
        }

        public void UnfollowAsync(string user,
                                  Action callback,
                                  Action<GitHubException> onError) {
            var resource = string.Format("/user/unfollow/{0}", user);
            var request = new GitHubRequest(resource, API.v2, Method.POST);
            _gitHubClient.CallApiAsync<object>(request,
                                               s => callback(),
                                               onError);
        }

        public void GetFollowersAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/followers?full=1", user);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    r => callback(r.Data.Users),
                                                    onError);
        }

        public void GetFollowingAsync(string user,
                                      Action<IEnumerable<User>> callback,
                                      Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/following?full=1", user);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _gitHubClient.CallApiAsync<UsersResult>(request,
                                                    r => callback(r.Data.Users),
                                                    onError);
        }

        public void GetWatchersAsync(string user,
                                     string repo,
                                     Action<IEnumerable<User>> callback,
                                     Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");
            Requires.ArgumentNotNull(repo, "repo");

            var resource = string.Format("/repos/show/{0}/{1}/watchers?full=1", user, repo);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _gitHubClient.CallApiAsync<WatchersResult>(request,
                                                       r => callback(r.Data.Watchers),
                                                       onError);
        }
    }
}
