using System;
using System.Collections.Generic;
using NGitHub.Models;
using NGitHub.Web;

namespace NGitHub.Services {
    public interface IFeedService {
        void GetUserActivityAsync(string user,
                                  Action<IEnumerable<FeedItem>> callback,
                                  Action<GitHubException> onError);
        void GetUserNewsFeedAsync(string user,
                                  Action<IEnumerable<FeedItem>> callback,
                                  Action<GitHubException> onError);
    }
}
