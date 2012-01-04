using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IOrganizationService {
        GitHubRequestAsyncHandle GetMembersAsync(string organization,
                                                 int page,
                                                 Action<IEnumerable<User>> callback,
                                                 Action<GitHubException> onError);
        GitHubRequestAsyncHandle GetOrganizationsAsync(string user,
                                                       int page,
                                                       Action<IEnumerable<User>> callback,
                                                       Action<GitHubException> onError);
    }
}
