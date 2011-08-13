using System;
using NGitHub.Utility;

namespace NGitHub {
    public class GitHubException : Exception {
        private readonly IGitHubResponse _response;

        public GitHubException(IGitHubResponse response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public IGitHubResponse Response {
            get {
                return _response;
            }
        }
    }
}
