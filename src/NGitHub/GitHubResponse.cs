using NGitHub.Utility;
using RestSharp;
using System;

namespace NGitHub {
    public class GitHubResponse<T> : GitHubResponseBase, IGitHubResponse<T> {
        private readonly IRestResponse<T> _response;

        public GitHubResponse(IRestResponse<T> response)
            : base(response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public T Data {
            get {
                return _response.Data;
            }
        }

        public string ErrorMessage {
            get {
                return _response.ErrorMessage;
            }
        }

        public Exception ErrorException {
            get {
                return _response.ErrorException;
            }
        }
    }

    public class GitHubResponse : GitHubResponseBase, IGitHubResponse {
        public GitHubResponse(IRestResponse response)
            : base(response) {
        }
    }
}