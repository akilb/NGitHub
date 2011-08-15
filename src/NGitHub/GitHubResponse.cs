using NGitHub.Utility;
using RestSharp;

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
    }

    public class GitHubResponse : GitHubResponseBase, IGitHubResponse {
        public GitHubResponse(IRestResponse response)
            : base(response) {
        }
    }
}