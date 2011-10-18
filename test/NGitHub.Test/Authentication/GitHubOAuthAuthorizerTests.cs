using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NGitHub.Authentication;
using NGitHub.Helpers;
using NGitHub.Web;
using RestSharp;

namespace NGitHub.Test.Authentication {
    [TestClass]
    public class GitHubOAuthAuthorizerTests {
        private readonly RestRequestAsyncHandle _testHandle = new RestRequestAsyncHandle();

        private GitHubOAuthAuthorizer CreateAuthorizer(IRestClientFactory factory = null,
                                                       IResponseProcessor processor = null) {
            return new GitHubOAuthAuthorizer(factory ?? new Mock<IRestClientFactory>(MockBehavior.Strict).Object,
                                             processor ?? new Mock<IResponseProcessor>(MockBehavior.Strict).Object);
        }

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

        [TestMethod]
        public void GetAccessTokenAsync_ShouldBuildRestClient_WithBaseAuthorizationUrl() {
            var expectedBaseUrl = Constants.AuthorizeUrl;
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(expectedBaseUrl))
                       .Returns(mockClient.Object)
                       .Verifiable();
            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle);
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync("foo", "bar", "baz", s => { }, e => { });

            mockFactory.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldBuildRestRequest_WithExpectedResource() {
            var expectedResource = "/access_token";
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object)
                       .Verifiable();
            mockClient.Setup(c => c.ExecuteAsync(It.Is<RestRequest>(r => r.Resource == expectedResource),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle);
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync("foo", "bar", "baz", s => { }, e => { });

            mockClient.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldPostRestRequest() {
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object)
                       .Verifiable();
            mockClient.Setup(c => c.ExecuteAsync(It.Is<RestRequest>(r => r.Method == RestSharp.Method.POST),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle);
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync("foo", "bar", "baz", s => { }, e => { });

            mockClient.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldAddClientIdToRequestParameters() {
            var expectedClientId = "id";
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.Is<RestRequest>(r => r.Parameters.Any(p => p.Name == "client_id" &&
                                                                                               (string)p.Value == expectedClientId)),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Verifiable();
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync(expectedClientId, "bar", "baz", s => { }, e => { });

            mockClient.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldAddClientSecretToRequestParameters() {
            var expectedClientSecret = "secret";
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.Is<RestRequest>(r => r.Parameters.Any(p => p.Name == "client_secret" &&
                                                                                               (string)p.Value == expectedClientSecret)),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Verifiable();
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync("foo", expectedClientSecret, "baz", s => { }, e => { });

            mockClient.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldAddCodeToRequestParameters() {
            var expectedCode = "code";
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.Is<RestRequest>(r => r.Parameters.Any(p => p.Name == "code" &&
                                                                                               (string)p.Value == expectedCode)),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Verifiable();
            var auth = CreateAuthorizer(mockFactory.Object);

            auth.GetAccessTokenAsync("foo", "bar", expectedCode, s => { }, e => { });

            mockClient.Verify();
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldCallOnError_IfRequestFails() {
            bool onErrorCalled = false;
            GitHubException ex = null;
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Callback<RestRequest, Action<RestResponse, RestRequestAsyncHandle>>(
                            (req, c) => c(new RestResponse(), _testHandle));
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<GitHubResponse>(), out ex))
                         .Returns(true);
            var auth = CreateAuthorizer(mockFactory.Object, mockProcessor.Object);

            auth.GetAccessTokenAsync("foo", "bar", "baz", s => { }, e => onErrorCalled = true);

            Assert.IsTrue(onErrorCalled);
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldPassExpectedError_IfRequestFails() {
            GitHubException expectedException = new GitHubException(new GitHubResponse(new RestResponse()), ErrorType.Unknown);
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Callback<RestRequest, Action<RestResponse, RestRequestAsyncHandle>>((req, c) => c(new RestResponse(), _testHandle));
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<GitHubResponse>(), out expectedException))
                         .Returns(true);
            var auth = CreateAuthorizer(mockFactory.Object, mockProcessor.Object);

            GitHubException actualException = null;
            auth.GetAccessTokenAsync("foo", "bar", "baz", s => { }, e => actualException = e);

            Assert.AreSame(expectedException, actualException);
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldCallCallback_IfRequestSucceeds() {
            var callbackCalled = false;
            var responseContent = "access_token=something";
            GitHubException ex = null;
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Callback<RestRequest, Action<RestResponse, RestRequestAsyncHandle>>(
                        (req, c) => c(new RestResponse { Content = responseContent }, _testHandle));
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<GitHubResponse>(), out ex))
                         .Returns(false);
            var auth = CreateAuthorizer(mockFactory.Object, mockProcessor.Object);

            auth.GetAccessTokenAsync("foo", "bar", "baz", t => callbackCalled = true, e => { });

            Assert.IsTrue(callbackCalled);
        }

        [TestMethod]
        public void GetAccessTokenAsync_ShouldPassAccessTokenToCallback_IfRequestSucceeds() {
            var expectedToken = "token";
            var responseContent = string.Format("access_token={0}&other_stuff=some%20crap", expectedToken);
            GitHubException ex = null;
            var mockClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockClient.Object);
            mockClient.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(),
                                                 It.IsAny<Action<RestResponse, RestRequestAsyncHandle>>()))
                      .Returns(_testHandle)
                      .Callback<RestRequest, Action<RestResponse, RestRequestAsyncHandle>>(
                            (req, c) => c(new RestResponse { Content = responseContent }, _testHandle));
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<GitHubResponse>(), out ex))
                         .Returns(false);
            var auth = CreateAuthorizer(mockFactory.Object, mockProcessor.Object);

            string actualToken = null;
            auth.GetAccessTokenAsync("foo", "bar", "baz", t => actualToken = t, e => { });

            Assert.AreEqual(expectedToken, actualToken);
        }
    }
}