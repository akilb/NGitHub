using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IOrganizationService {
        void GetMembersAsync(string organization,
                             int page,
                             Action<IEnumerable<User>> callback,
                             Action<GitHubException> onError);
        void GetOrganizationsAsync(string user,
                                   int page,
                                   Action<IEnumerable<User>> callback,
                                   Action<GitHubException> onError);
    }
}
