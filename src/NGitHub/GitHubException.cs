using System;
using NGitHub.Utility;
using NGitHub.Web;

namespace NGitHub {
    public class GitHubException : Exception {
        private readonly IGitHubResponse _response;
        private readonly ErrorType _errorType;

        public GitHubException(IGitHubResponse response,
                               ErrorType errorType)
            : base(string.Empty, response.ErrorException) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
            _errorType = errorType;
        }

        public IGitHubResponse Response {
            get {
                return _response;
            }
        }

        public ErrorType ErrorType {
            get {
                return _errorType;
            }
        }
    }
}
