using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using Moq;
using System.Net;

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
        public void IsError_ShouldBeTrue_IfResponseStatusCodeIsNotOkOrCreated() {
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.StatusCode)
                    .Returns(HttpStatusCode.Forbidden);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.IsTrue(resp.IsError);
        }

        [TestMethod]
        public void IsError_ShouldBeFalse_IfResponseStatusCodeIsOk() {
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.StatusCode)
                    .Returns(HttpStatusCode.OK);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.IsFalse(resp.IsError);
        }

        [TestMethod]
        public void IsError_ShouldBeFalse_IfResponseStatusCodeIsCreated() {
            var mockResp = new Mock<IRestResponse<object>>(MockBehavior.Strict);
            mockResp.Setup(r => r.StatusCode)
                    .Returns(HttpStatusCode.Created);
            var resp = new GitHubResponse<object>(mockResp.Object);

            Assert.IsFalse(resp.IsError);
        }
    }
}