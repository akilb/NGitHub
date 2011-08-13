using System;
using System.Collections.Generic;
using NGitHub.Models;

namespace NGitHub {
    public interface IOrganizationService {
        void GetMembersAsync(string organization,
                             Action<IEnumerable<User>> callback,
                             Action<GitHubException> onError);
        void GetOrganizationsAsync(string user,
                                   Action<IEnumerable<User>> callback,
                                   Action<GitHubException> onError);
    }
}
