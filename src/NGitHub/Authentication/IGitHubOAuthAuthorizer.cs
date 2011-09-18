using System;

namespace NGitHub.Authentication {
    public interface IGitHubOAuthAuthorizer {
        string BuildAuthenticationUrl(string clientId,
                                      string redirectUrl,
                                      params Scope[] scopes);
        void GetAccessTokenAsync(string clientId,
                                 string clientSecret,
                                 string code,
                                 Action<string> callback,
                                 Action<GitHubException> onError);
    }
}