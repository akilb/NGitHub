using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NGitHub.Web;
using RestSharp;

namespace NGitHub.Test {
    [TestClass]
    public class GitHubResponseTests {
        [TestMethod]
        public void Data_ShouldContainTheResponseData() {
            var expectedData = new object();
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.Data)
                    .Returns(expectedData);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreSame(expectedData, resp.Data);
        }

        [TestMethod]
        public void StatusCode_ShouldContainResponseStatus() {
            var expectedStatusCode = HttpStatusCode.Conflict;
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.StatusCode)
                    .Returns(expectedStatusCode);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual(expectedStatusCode, resp.StatusCode);
        }

        [TestMethod]
        public void ErrorMessage_ShouldContainTheResponseErrorMessage() {
            var expectedErrorMessage = "foo";
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.ErrorMessage)
                    .Returns(expectedErrorMessage);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual(expectedErrorMessage, resp.ErrorMessage);
        }

        [TestMethod]
        public void ErrorException_ShouldContainTheResponseErrorException() {
            var expectedException = new Exception();
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.ErrorException)
                    .Returns(expectedException);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual<Exception>(expectedException, resp.ErrorException);
        }

        [TestMethod]
        public void Content_ShouldReturnResponseContent() {
            var expectedContent = "foo";
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.Content)
                    .Returns(expectedContent);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual(expectedContent, resp.Content);
        }

        [TestMethod]
        public void ContentType_ShouldReturnResponseContentType() {
            var expectedContentType = "foo";
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.ContentType)
                    .Returns(expectedContentType);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual(expectedContentType, resp.ContentType);
        }

        [TestMethod]
        public void ResponseStatus_ShouldReturnTheConvertedResponseResponseStatus() {
            var expectedResponseStatus = NGitHub.Web.ResponseStatus.Completed;
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.ResponseStatus)
                    .Returns(RestSharp.ResponseStatus.Completed);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.AreEqual<NGitHub.Web.ResponseStatus>(expectedResponseStatus, resp.ResponseStatus);
        }
    }
}