using System.Net;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class GitHubResponse<T> : IGitHubResponse<T> {
        private readonly IRestResponse<T> _response;

        public GitHubResponse(IRestResponse<T> response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public T Data {
            get {
                return _response.Data;
            }
        }

        public HttpStatusCode StatusCode {
            get {
                return _response.StatusCode;
            }
        }

        public bool IsError {
            get {
                return _response.StatusCode != HttpStatusCode.OK &&
                       _response.StatusCode != HttpStatusCode.Created;
            }
        }
    }
}