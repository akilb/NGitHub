namespace NGitHub.Authentication {
    interface IGitHubOAuthAuthorizer {
        string BuildAuthenticationUrl(string clientId,
                                      string redirectUrl,
                                      params Scope[] scopes);
    }
}