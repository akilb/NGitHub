using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGitHub.Services;
using Moq;
using NGitHub.Models;
using NGitHub.Test.Helpers;
using RestSharp;

namespace NGitHub.Test.Services {
    [TestClass]
    public class IssueServiceTests {
        [TestMethod]
        public void CreateCommentAsync_ShouldAddComment_WithBodySetToCommentText_AsRequestBody() {
            var expectedBody = "fooBody";
            object requestBody = null;
            var mockClient = new Mock<IGitHubClient>(MockBehavior.Strict);
            mockClient.Setup(c => c.CallApiAsync(It.IsAny<GitHubRequest>(),
                                                 It.IsAny<Action<IGitHubResponse<Comment>>>(),
                                                 It.IsAny<Action<GitHubException>>()))
                      .Callback<GitHubRequest, Action<IGitHubResponse<Comment>>, Action<GitHubException>>(
                        (req, c, e) => requestBody = req.Body)
                      .Returns(TestHelpers.CreateTestHandle())
                      .Verifiable();
            var svc = new IssueService(mockClient.Object);

            svc.CreateCommentAsync("foo", "bar", 1, expectedBody, c => { }, e => { });

            var actualBody = ((dynamic)requestBody).body;
            Assert.AreSame(expectedBody, actualBody);
        }
    }
}
