using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGitHub.Authentication;

namespace NGitHub.Test.Authentication {
    [TestClass]
    public class GitHubOAuthAuthorizerTests {
        [TestMethod]
        public void BuildAuthenticationUrl_WithNoScopesDefined() {
            var clientId = "foo";
            var redirectUrl = "bar";
            var expectedUrl = string.Format("{0}?client_id={1}&redirect_uri={2}",
                                            Constants.AuthenticationUrl,
                                            clientId,
                                            redirectUrl);
            var auth = new GitHubOAuthAuthorizer();

            var actualUrl = auth.BuildAuthenticationUrl(clientId, redirectUrl);

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [TestMethod]
        public void BuildAuthenticationUrl_WithAScopeDefined() {
            var clientId = "foo";
            var redirectUrl = "bar";
            var scopes = new Scope[] { Scope.PublicRepo };
            var expectedUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}",
                                            Constants.AuthenticationUrl,
                                            clientId,
                                            redirectUrl,
                                            "public_repo");
            var auth = new GitHubOAuthAuthorizer();

            var actualUrl = auth.BuildAuthenticationUrl(clientId, redirectUrl, scopes);

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [TestMethod]
        public void BuildAuthenticationUrl_WithMultipleScopesDefined() {
            var clientId = "foo";
            var redirectUrl = "bar";
            var scopes = new Scope[] { Scope.PublicRepo, Scope.Repo, Scope.Gists, Scope.User };
            var expectedUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}",
                                            Constants.AuthenticationUrl,
                                            clientId,
                                            redirectUrl,
                                            "public_repo,repo,gists,user");
            var auth = new GitHubOAuthAuthorizer();

            var actualUrl = auth.BuildAuthenticationUrl(clientId, redirectUrl, scopes);

            Assert.AreEqual(expectedUrl, actualUrl);
        }

        [TestMethod]
        public void BuildAuthenticationUrl_WithDuplicateScopesDefined() {
            var clientId = "foo";
            var redirectUrl = "bar";
            var scopes = new Scope[] { Scope.PublicRepo, Scope.PublicRepo };
            var expectedUrl = string.Format("{0}?client_id={1}&redirect_uri={2}&scope={3}",
                                            Constants.AuthenticationUrl,
                                            clientId,
                                            redirectUrl,
                                            "public_repo");
            var auth = new GitHubOAuthAuthorizer();

            var actualUrl = auth.BuildAuthenticationUrl(clientId, redirectUrl, scopes);

            Assert.AreEqual(expectedUrl, actualUrl);
        }
    }
}