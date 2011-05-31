using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

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
            _client.CallApiAsync<UsersResult>(resource,
                                              API.v2,
                                              Method.GET,
                                              u => callback(u.Users),
                                              onError);
        }

        public void GetOrganizationsAsync(string user,
                                          Action<IEnumerable<User>> callback,
                                          Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/user/show/{0}/organizations", user);
            _client.CallApiAsync<OrganizationsResult>(resource,
                                                      API.v2,
                                                      Method.GET,
                                                      o => callback(o.Organizations),
                                                      onError);
        }
    }
}
