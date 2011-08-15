using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NGitHub.Web;

namespace NGitHub.Test.Web {
    [TestClass]
    public class GitHubExceptionTests {
        [TestMethod]
        public void Response_ShouldBeTheGivenResponse() {
            var mockResponse = new Mock<IGitHubResponse>(MockBehavior.Strict);
            mockResponse.Setup(r => r.ErrorException).Returns<Exception>(null);
            var expectedResponse = mockResponse.Object;
            var ex = new GitHubException(expectedResponse, ErrorType.Unknown);

            Assert.AreSame(expectedResponse, ex.Response);
        }

        [TestMethod]
        public void InnerException_ShouldBeTheErrorExceptionOfTheResponse() {
            var expectedInnerException = new Exception();
            var mockResponse = new Mock<IGitHubResponse>(MockBehavior.Strict);
            mockResponse.Setup(e => e.ErrorException)
                        .Returns(expectedInnerException);
            var ex = new GitHubException(mockResponse.Object, ErrorType.Unknown);

            Assert.AreSame(expectedInnerException, ex.InnerException);
        }

        [TestMethod]
        public void ErrorType_ShouldBeTheGivenErrorType() {
            var expectedErrorType = ErrorType.ServerError;
            var mockResponse = new Mock<IGitHubResponse>(MockBehavior.Strict);
            mockResponse.Setup(r => r.ErrorException).Returns<Exception>(null);
            var ex = new GitHubException(mockResponse.Object, expectedErrorType);

            Assert.AreEqual<ErrorType>(expectedErrorType, ex.ErrorType);
        }
    }
}
