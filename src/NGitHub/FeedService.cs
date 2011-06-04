using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using NGitHub.Models;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class FeedService : IFeedService {
        private readonly IRestClientFactory _factory;
        private readonly Func<IAuthenticator> _currentAuthenticator;

        public FeedService(IRestClientFactory factory, Func<IAuthenticator> currentAuthenticator) {
            Requires.ArgumentNotNull(factory, "factory");
            Requires.ArgumentNotNull(currentAuthenticator, "currentAuthenticator");

            _factory = factory;
            _currentAuthenticator = currentAuthenticator;
        }

        public void GetUserActivityAsync(string user,
                                         Action<IEnumerable<FeedItem>> callback,
                                         Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            GetFeedItemsAsync(string.Format("{0}.atom", user), callback, onError);
        }

        public void GetUserNewsFeedAsync(string user,
                                         Action<IEnumerable<FeedItem>> callback,
                                         Action<APICallError> onError) {
            Requires.ArgumentNotNull(user, "user");

            GetFeedItemsAsync(string.Format("{0}.private.atom", user), callback, onError);
        }

        private void GetFeedItemsAsync(string resource,
                                       Action<IEnumerable<FeedItem>> callback,
                                       Action<APICallError> onError) {
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(onError, "onError");

            var client = _factory.CreateRestClient(Constants.GitHubUrl);
            client.Authenticator = _currentAuthenticator();

            var request = new RestRequest(resource, Method.GET);
            client.ExecuteAsync(request,
                                r => {
                                    if (r.StatusCode != HttpStatusCode.OK) {
                                        onError(new APICallError(r));
                                        return;
                                    }

                                    using (var tr = new StringReader(r.Content))
                                    using (var reader = XmlReader.Create(tr)) {
                                        var feedItems = SyndicationFeed.Load(reader).Items.Select(i => new FeedItem(i));
                                        callback(feedItems ?? new List<FeedItem>());
                                    }
                                });
        }
    }
}
