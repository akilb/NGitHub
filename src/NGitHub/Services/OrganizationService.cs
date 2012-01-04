using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub.Services {
    public class OrganizationService : IOrganizationService {
        private readonly IGitHubClient _client;

        public OrganizationService(IGitHubClient gitHubClient) {
            Requires.ArgumentNotNull(gitHubClient, "gitHubClient");

            _client = gitHubClient;
        }

        public GitHubRequestAsyncHandle GetMembersAsync(string organization,
                                                        int page,
                                                        Action<IEnumerable<User>> callback,
                                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(organization, "organization");

            var resource = string.Format("/orgs/{0}/members", organization);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            return _client.CallApiAsync<List<User>>(request,
                                                    r => callback(r.Data),
                                                    onError);
        }

        public GitHubRequestAsyncHandle GetOrganizationsAsync(string user,
                                                              int page,
                                                              Action<IEnumerable<User>> callback,
                                                              Action<GitHubException> onError) {
            Requires.ArgumentNotNull(user, "user");

            var resource = string.Format("/users/{0}/orgs", user);
            var request = new GitHubRequest(resource,
                                            API.v3,
                                            Method.GET,
                                            Parameter.Page(page));
            return _client.CallApiAsync<List<User>>(request,
                                                    r => callback(r.Data),
                                                    onError);
        }
    }
}
