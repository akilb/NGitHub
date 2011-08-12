using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;

namespace NGitHub {
    public class OrganizationService : IOrganizationService {
        private readonly IGitHubClient _client;

        public OrganizationService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public void GetMembersAsync(string organization,
                                    Action<IEnumerable<User>> callback,
                                    Action<APICallError> onError) {
            Requires.ArgumentNotNull(organization, "organization");

            var resource = string.Format("/organizations/{0}/public_members",
                                         organization);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<UsersResult>(request,
                                              r => callback(r.Data.Users),
                                              onError);
        }

        public void GetOrganizationsAsync(string user,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/organizations", user);
            var request = new GitHubRequest(resource, API.v2, Method.GET);
            _client.CallApiAsync<OrganizationsResult>(request,
                                                      r => callback(r.Data.Organizations),
                                                      onError);
        }
    }
}
