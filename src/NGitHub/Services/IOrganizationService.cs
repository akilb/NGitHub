using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IOrganizationService {
        GitHubRequestAsyncHandle GetMembersAsync(string organization,
                                                 Action<IEnumerable<User>> callback,
                                                 Action<GitHubException> onError);
        GitHubRequestAsyncHandle GetOrganizationsAsync(string user,
                                                       Action<IEnumerable<User>> callback,
                                                       Action<GitHubException> onError);
    }
}
