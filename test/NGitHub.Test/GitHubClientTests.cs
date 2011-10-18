using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NGitHub.Authentication;
using NGitHub.Helpers;
using NGitHub.Web;
using RestSharp;

namespace NGitHub.Test {
    [TestClass]
    public class GitHubClientTests {
        private RestRequestAsyncHandle _testHandle = new RestRequestAsyncHandle();

        private GitHubClient CreateClient(IRestClientFactory factory = null,
                                          IResponseProcessor processor = null) {
            if (processor == null) {
                GitHubException ex = null;
                var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
                mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<IGitHubResponse>(),
                                                                    out ex))
                             .Returns(false);
                processor = mockProcessor.Object;
            }
            return new GitHubClient(factory ?? new Mock<IRestClientFactory>(MockBehavior.Strict).Object,
                                    processor);
        }

        [TestMethod]
        public void CallApiAsync_ShouldExecuteRestRequestWithGivenRequestResource() {
            var expectedResource = "foo/bar";
            var request = new GitHubRequest(expectedResource, API.v3, NGitHub.Web.Method.OPTIONS);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.Is<IRestRequest>(r => r.Resource == expectedResource),
                It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle);
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = CreateClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(request, o => { }, e => { });

            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldExecuteRestRequestWithGivenRequestMethod() {
            var expectedMethod = NGitHub.Web.Method.OPTIONS;
            var request = new GitHubRequest("foo/bar", API.v3, expectedMethod);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.Is<IRestRequest>(r => r.Method == expectedMethod.ToRestSharpMethod()),
                It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle);
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = CreateClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(request, o => { }, e => { });

            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldUseV2BaseUrl_WhenVersion2IsSpecified() {
            var expectedBaseUrl = Constants.ApiV2Url;
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(expectedBaseUrl))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.IsAny<IRestRequest>(),
                It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle);
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = CreateClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(new GitHubRequest("foo", API.v2, NGitHub.Web.Method.GET),
                                              o => { },
                                              e => { });

            mockFactory.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldUseV3BaseUrl_WhenVersion3IsSpecified() {
            var expectedBaseUrl = Constants.ApiV3Url;
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(expectedBaseUrl))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(),
                                                             It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                          .Returns(_testHandle);
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = CreateClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                                              o => { },
                                              e => { });

            mockFactory.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldInvokeCallbackMethod_WhenRestRequestCompletesSuccessfully() {
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var response = new RestResponse<object>() { StatusCode = HttpStatusCode.OK };
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(
                    It.IsAny<IRestRequest>(),
                    It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>,
                                 RestRequestAsyncHandle>>((r, c) => c(response, _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = CreateClient(mockFactory.Object);

            var callbackInvoked = false;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                o => callbackInvoked = true,
                e => { });

            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        public void CallApiAsync_ShouldInvokeCallBackMethod_WhenRequestHasCreatedStatus() {
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var response = new RestResponse<object>() { StatusCode = HttpStatusCode.Created };
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(), It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>, RestRequestAsyncHandle>>((r, c) => c(response, _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = CreateClient(mockFactory.Object);

            var callbackInvoked = false;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                o => callbackInvoked = true,
                e => { });

            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        public void CallApiAsync_ShouldPassResponseDataToCallback_WhenRestRequestCompletesSuccessfully() {
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var expectedData = new object();
            var response = new RestResponse<object>() {
                StatusCode = HttpStatusCode.OK,
                Data = expectedData
            };
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(),
                                                   It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>,
                          RestRequestAsyncHandle>>((r, c) => c(response, _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = CreateClient(mockFactory.Object);

            object actualData = null;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                r => actualData = r.Data,
                e => { });

            Assert.AreSame(expectedData, actualData);
        }

        [TestMethod]
        public void CallApiAsync_ShouldCallOnError_IfRestRequestDoesNotCompleteSuccessfully() {
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var response = new RestResponse<object>();
            var exception = new GitHubException(new GitHubResponse(response), ErrorType.NoNetwork);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>())).Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(),
                                                   It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>,
                          RestRequestAsyncHandle>>((r, c) => c(response, _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<IGitHubResponse>(),
                                                                out exception))
                         .Returns(true);
            var client = CreateClient(mockFactory.Object, mockProcessor.Object);

            var onErrorInvoked = false;
            client.CallApiAsync<object>(new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                                        o => { },
                                        e => onErrorInvoked = true);

            Assert.IsTrue(onErrorInvoked);
        }

        [TestMethod]
        public void CallApiAsync_ShouldPassExceptionToOnError_IfThereAreResponseErrors() {
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockProcessor = new Mock<IResponseProcessor>(MockBehavior.Strict);
            var response = new RestResponse<object>();
            var expectedException = new GitHubException(new GitHubResponse(response), ErrorType.NoNetwork);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>())).Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(), It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>,
                          RestRequestAsyncHandle>>((r, c) => c(response, _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            mockProcessor.Setup(p => p.TryProcessResponseErrors(It.IsAny<IGitHubResponse>(),
                                                                out expectedException))
                         .Returns(true);
            var client = CreateClient(mockFactory.Object, mockProcessor.Object);

            GitHubException actualException = null;
            client.CallApiAsync<object>(new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                                        o => { },
                                        e => actualException = e);

            Assert.AreEqual(expectedException, actualException);
        }

        [TestMethod]
        public void CallApiAsync_ShouldPassRequestParameters_ToRestRequest() {
            var expectedKey = "foo";
            var expectedValue = "bar";
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>())).Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(
                    It.Is<IRestRequest>(r => r.Parameters.Where(p => (p.Name == expectedKey) &&
                                                                    ((string)p.Value == expectedValue))
                                                        .Count() == 1),
                    It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle)
                .Callback<IRestRequest,
                          Action<RestResponse<object>,
                                 RestRequestAsyncHandle>>((r, c) => c(new RestResponse<object>(), _testHandle));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var client = CreateClient(mockFactory.Object);

            var request = new GitHubRequest("resource", API.v2,
                                            NGitHub.Web.Method.POST,
                                            new NGitHub.Web.Parameter(expectedKey, expectedValue));
            client.CallApiAsync<object>(request, o => { }, e => { });

            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public void Authenticator_ShouldBeAssignedToNullAuthenticator_WhenClientIsCreated() {
            var client = CreateClient();

            Assert.IsInstanceOfType(client.Authenticator, typeof(NullAuthenticator));
        }

        [TestMethod]
        public void Authenticator_ShouldTakeAssignedValue_WhenItIsNotNull() {
            var expectedAuthenticator = new SimpleAuthenticator(string.Empty, string.Empty, string.Empty, string.Empty);
            var client = CreateClient();

            client.Authenticator = expectedAuthenticator;

            Assert.AreSame(expectedAuthenticator, client.Authenticator);
        }

        [TestMethod]
        public void Authenticator_ShouldDefaultToNullAuthenticator_WhenAssignedToNull() {
            var client = CreateClient();

            client.Authenticator = null;

            Assert.IsInstanceOfType(client.Authenticator, typeof(NullAuthenticator));
        }

        [TestMethod]
        public void CallApiAsync_ShouldUseGivenAuthenticator_DuringForRestClient() {
            var expectedAuthenticator = new NullAuthenticator();
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>()))
                       .Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<IRestRequest>(),
                                                   It.IsAny<Action<RestResponse<object>, RestRequestAsyncHandle>>()))
                .Returns(_testHandle);
            mockRestClient.SetupSet(c => c.Authenticator = expectedAuthenticator)
                          .Verifiable();
            var client = CreateClient(mockFactory.Object);
            client.Authenticator = expectedAuthenticator;

            client.CallApiAsync<object>(new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                                        o => { },
                                        e => { });

            mockRestClient.Verify();
        }
    }
}