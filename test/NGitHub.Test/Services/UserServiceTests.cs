using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Moq;
using NGitHub.Models;
using System.Net;
using NGitHub.Web;
using NGitHub.Services;
using NGitHub.Test.Helpers;

namespace NGitHub.Test.Services {
    [TestClass]
    public class UserServiceTests {
        [TestMethod]
        public void IsFollowingAsync_ShouldCallbackWithTrue_WhenResponseIsNoContent() {
            var mockResponse = new Mock<IGitHubResponse<object>>(MockBehavior.Strict);
            var mockClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockResponse.Setup(r => r.ErrorException)
                        .Returns(new Exception());
            mockResponse.Setup(r => r.StatusCode)
                        .Returns(HttpStatusCode.NoContent);
            mockClient.Setup(c => c.CallApiAsync<object>(It.IsAny<GitHubRequest>(),
                                                         It.IsAny<Action<IGitHubResponse<object>>>(),
                                                         It.IsAny<Action<GitHubException>>()))
                      .Callback<GitHubRequest,
                                Action<IGitHubResponse<object>>,
                                Action<GitHubException>>((req, c, e) => {
                                    e(new GitHubException(mockResponse.Object, ErrorType.Unknown));
                                })
                      .Returns(TestHelpers.CreateTestHandle());
            var userService = new UserService(mockClient.Object);

            var isFollowing = false;
            userService.IsFollowingAsync("akilb",
                                         fl => isFollowing = fl,
                                         e => { });

            Assert.IsTrue(isFollowing);
        }

        [TestMethod]
        public void IsFollowingAsync_ShouldCallbackWithFalse_WhenResponseIsNotFound() {
            var mockResponse = new Mock<IGitHubResponse<object>>(MockBehavior.Strict);
            var mockClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockResponse.Setup(r => r.ErrorException)
                        .Returns(new Exception());
            mockResponse.Setup(r => r.StatusCode)
                        .Returns(HttpStatusCode.NotFound);
            mockClient.Setup(c => c.CallApiAsync<object>(It.IsAny<GitHubRequest>(),
                                                         It.IsAny<Action<IGitHubResponse<object>>>(),
                                                         It.IsAny<Action<GitHubException>>()))
                      .Callback<GitHubRequest,
                                Action<IGitHubResponse<object>>,
                                Action<GitHubException>>((req, c, e) => {
                                    e(new GitHubException(mockResponse.Object, ErrorType.ResourceNotFound));
                                })
                      .Returns(TestHelpers.CreateTestHandle());
            var userService = new UserService(mockClient.Object);

            var isFollowing = true;
            userService.IsFollowingAsync("akilb",
                                         fl => isFollowing = fl,
                                         e => { });

            Assert.IsFalse(isFollowing);
        }

        [TestMethod]
        public void IsFollowingAsync_ShouldCallbackWithError_WhenResponseIsSomeRandomError() {
            var mockResponse = new Mock<IGitHubResponse<object>>(MockBehavior.Strict);
            var mockClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockResponse.Setup(r => r.ErrorException)
                        .Returns(new Exception());
            mockResponse.Setup(r => r.StatusCode)
                        .Returns(HttpStatusCode.Forbidden);
            var expectedException = new GitHubException(mockResponse.Object,
                                                        ErrorType.Unauthorized);
            mockClient.Setup(c => c.CallApiAsync<object>(It.IsAny<GitHubRequest>(),
                                                         It.IsAny<Action<IGitHubResponse<object>>>(),
                                                         It.IsAny<Action<GitHubException>>()))
                      .Callback<GitHubRequest,
                                Action<IGitHubResponse<object>>,
                                Action<GitHubException>>((req, c, e) => {
                                    e(expectedException);
                                })
                      .Returns(TestHelpers.CreateTestHandle());
            var userService = new UserService(mockClient.Object);

            GitHubException actualException = null;
            userService.IsFollowingAsync("akilb",
                                         c => { },
                                         e => actualException = e);

            Assert.AreSame(expectedException, actualException);
        }
    }
}
