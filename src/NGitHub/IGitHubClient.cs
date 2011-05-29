using System;
using RestSharp;

namespace NGitHub {
    public interface IGitHubClient {
        ICommentService Comments { get; }
        IIssueService Issues { get; }
        IUserService Users { get; }

        void CallApiAsync<TJsonResponse>(string resource,
                                         API version,
                                         Method method,
                                         Action<TJsonResponse> callback,
                                         Action<APICallError> onError) where TJsonResponse : new();
    }
}
