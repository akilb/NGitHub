using System;
using System.Linq;
using System.Text;
using NGitHub.Helpers;
using NGitHub.Utility;

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
    }
}