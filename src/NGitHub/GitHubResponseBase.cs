using System.Net;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public abstract class GitHubResponseBase {
        private readonly IRestResponse _response;

        public GitHubResponseBase(IRestResponse response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
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
