using System;
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
            Requires.ArgumentNotNull(callback, "callback");

            var resource = string.Format("/users/{0}", user);
            _gitHubClient.CallApiAsync<User>(resource, API.Version3, Method.GET, callback, onError);
        }
    }
}
