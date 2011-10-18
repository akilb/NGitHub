using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NGitHub.Helpers;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub.Authentication {
    public class GitHubOAuthAuthorizer : IGitHubOAuthAuthorizer {
        private readonly IRestClientFactory _factory;
        private readonly IResponseProcessor _processor;

        public GitHubOAuthAuthorizer()
            : this(new RestClientFactory(), new ResponseProcessor()) {
        }

        public GitHubOAuthAuthorizer(IRestClientFactory factory,
                                     IResponseProcessor processor) {
            Requires.ArgumentNotNull(factory, "factory");
            Requires.ArgumentNotNull(processor, "processor");

            _factory = factory;
            _processor = processor;
        }

        public string BuildAuthenticationUrl(string clientId,
                                             string redirectUrl,
                                             params Scope[] scopes) {
            Requires.ArgumentNotNull(clientId, "clientId");
            Requires.ArgumentNotNull(redirectUrl, "redirectUrl");
            Requires.ArgumentNotNull(scopes, "scopes");

            var url = string.Format("{0}?client_id={1}&redirect_uri={2}{3}",
                                    Constants.AuthenticationUrl,
                                    clientId,
                                    redirectUrl,
                                    BuildScopeText(scopes));

            return url;
        }

        private string BuildScopeText(Scope[] scopes) {
            if (scopes.Length == 0) {
                return string.Empty;
            }

            var scopeBuilder = new StringBuilder();
            bool isFirstScope = true;
            foreach (var scope in scopes.Distinct()) {
                if (isFirstScope) {
                    isFirstScope = false;
                    scopeBuilder.Append("&scope=");
                }
                else {
                    scopeBuilder.Append(",");
                }

                string scopeText = null;
                switch (scope) {
                    case Scope.User:
                        scopeText = "user";
                        break;
                    case Scope.PublicRepo:
                        scopeText = "public_repo";
                        break;
                    case Scope.Repo:
                        scopeText = "repo";
                        break;
                    case Scope.Gists:
                        scopeText = "gists";
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                scopeBuilder.Append(scopeText);
            }

            return scopeBuilder.ToString();
        }

        public void GetAccessTokenAsync(string clientId,
                                        string clientSecret,
                                        string code,
                                        Action<string> callback,
                                        Action<GitHubException> onError) {
            Requires.ArgumentNotNull(clientId, "clientId");
            Requires.ArgumentNotNull(clientSecret, "clientSecret");
            Requires.ArgumentNotNull(code, "code");
            Requires.ArgumentNotNull(callback, "callback");
            Requires.ArgumentNotNull(onError, "onError");

            var request = new RestRequest {
                Resource = "/access_token",
                Method = Method.POST
            };
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("code", code);

            var client = _factory.CreateRestClient(Constants.AuthorizeUrl);

            client.ExecuteAsync(
                request,
                (r, h) => {
                    var response = new GitHubResponse(r);

                    GitHubException ex = null;
                    if (_processor.TryProcessResponseErrors(response, out ex)) {
                        onError(ex);
                        return;
                    }

                    var parameters = response.Content.Split('&');
                    var accessToken = parameters.Where(p => p.StartsWith("access_token="))
                                                .Select(p => p.Substring(("access_token=").Length))
                                                .FirstOrDefault();

                    Debug.Assert(accessToken != null, "");

                    callback(accessToken);
                });
        }
    }
}