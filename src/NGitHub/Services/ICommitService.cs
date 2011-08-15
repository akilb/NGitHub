using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface ICommitService {
        void GetCommitsAsync(string user,
                             string repo,
                             string branch,
                             int pageNo,
                             Action<IEnumerable<Commit>> callback,
                             Action<GitHubException> onError);
    }
}
