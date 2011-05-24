using System;
using RestSharp;

namespace NGitHub {
    public interface IGitHubClient {
        IUserService Users { get; }

        void CallApiAsync<TJsonResponse>(string resource,
                                         Method method,
                                         Action<TJsonResponse> callback,
                                         Action<APICallError> onError) where TJsonResponse : new();
    }
}
