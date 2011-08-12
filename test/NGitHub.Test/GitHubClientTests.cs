using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using System.Linq;

namespace NGitHub.Test {
    [TestClass]
    public class GitHubClientTests {
        [TestMethod]
        public void CallApiAsync_ShouldExecuteRestRequestWithGivenRequestResource() {
            var expectedResource = "foo/bar";
            var request = new GitHubRequest(expectedResource, API.v3, Method.OPTIONS);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.Is<RestRequest>(r => r.Resource == expectedResource),
                It.IsAny<Action<RestResponse<object>>>()));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = new GitHubClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(request, o => { }, e => { });

            mockRestClient.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldExecuteRestRequestWithGivenRequestMethod() {
            var expectedMethod = Method.OPTIONS;
            var request = new GitHubRequest("foo/bar", API.v3, expectedMethod);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url))
                       .Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.Is<RestRequest>(r => r.Method == expectedMethod.ToRestSharpMethod()),
                It.IsAny<Action<RestResponse<object>>>()));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = new GitHubClient(mockFactory.Object);

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
                It.IsAny<RestRequest>(),
                It.IsAny<Action<RestResponse<object>>>()));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = new GitHubClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(new GitHubRequest("foo", API.v2, Method.GET),
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
            mockRestClient.Setup(c => c.ExecuteAsync<object>(
                It.IsAny<RestRequest>(),
                It.IsAny<Action<RestResponse<object>>>()));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var githubClient = new GitHubClient(mockFactory.Object);

            githubClient.CallApiAsync<object>(new GitHubRequest("foo", API.v3, Method.GET),
                                              o => { },
                                              e => { });

            mockFactory.VerifyAll();
        }

        [TestMethod]
        public void CallApiAsync_ShouldInvokeCallbackMethod_WhenRestRequestCompletesSuccessfully() {
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var response = new RestResponse<object>() { StatusCode = HttpStatusCode.OK };
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(response));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = new GitHubClient(mockFactory.Object);

            var callbackInvoked = false;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, Method.GET),
                o => callbackInvoked = true,
                e => { });

            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        public void CallApiAsync_ShouldInvokeCallBackMethod_WhenRequestHasCreatedStatus() {
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var response = new RestResponse<object>() { StatusCode = HttpStatusCode.Created };
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(response));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = new GitHubClient(mockFactory.Object);

            var callbackInvoked = false;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, Method.GET),
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
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(response));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(Constants.ApiV3Url)).Returns(mockRestClient.Object);

            var client = new GitHubClient(mockFactory.Object);

            object actualData = null;
            client.CallApiAsync<object>(
                new GitHubRequest("foo", API.v3, Method.GET),
                o => actualData = o,
                e => { });

            Assert.AreSame(expectedData, actualData);
        }

        [TestMethod]
        public void CallApiAsync_ShouldCallOnError_IfRestRequestDoesNotCompleteSuccessfully() {
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var errorResponse = new RestResponse<object>() { StatusCode = HttpStatusCode.Unauthorized };
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>())).Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(errorResponse));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var client = new GitHubClient(mockFactory.Object);

            var onErrorInvoked = false;
            client.CallApiAsync<object>(new GitHubRequest("foo", API.v3, Method.GET),
                                        o => { },
                                        e => onErrorInvoked = true);

            Assert.IsTrue(onErrorInvoked);
        }

        [TestMethod]
        public void CallApiAsync_ShouldPassResponseInErrorCode_IfRestRequestDoesNotCompleteSuccessfully() {
            var mockFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            var mockRestClient = new Mock<IRestClient>(MockBehavior.Strict);
            var expectedErrorResponse = new RestResponse<object>() { StatusCode = HttpStatusCode.Unauthorized };
            mockFactory.Setup<IRestClient>(f => f.CreateRestClient(It.IsAny<string>())).Returns(mockRestClient.Object);
            mockRestClient
                .Setup(c => c.ExecuteAsync<object>(It.IsAny<RestRequest>(), It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(expectedErrorResponse));
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var client = new GitHubClient(mockFactory.Object);

            IRestResponse actualErrorResponse = null;
            client.CallApiAsync<object>(new GitHubRequest("foo", API.v3, Method.GET),
                                        o => { },
                                        e => actualErrorResponse = e.Response);

            Assert.AreSame(expectedErrorResponse, actualErrorResponse);
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
                    It.Is<RestRequest>(r => r.Parameters.Where(p => p.Name == expectedKey && p.Value == expectedValue)
                                                        .Count() == 1),
                    It.IsAny<Action<RestResponse<object>>>()))
                .Callback<RestRequest, Action<RestResponse<object>>>((r, c) => c(new RestResponse<object>()))
                .Verifiable();
            mockRestClient.SetupSet(c => c.Authenticator = It.IsAny<IAuthenticator>());
            var client = new GitHubClient(mockFactory.Object);

            var request = new GitHubRequest("resource", API.v2, Method.POST, new Parameter(expectedKey, expectedValue));
            client.CallApiAsync<object>(request, o => { }, e => { });

            mockRestClient.Verify();
        }

        [TestMethod]
        public void LoggedIn_ShouldBeFalse_WhenClientHasNotTriedToLogInYet() {
            var client = new GitHubClient();

            Assert.IsFalse(client.LoggedIn);
        }
    }
}