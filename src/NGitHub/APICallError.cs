using NGitHub.Utility;
using RestSharp;

namespace NGitHub {
    public class APICallError {
        private readonly IRestResponse _response;

        public APICallError(IRestResponse response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public IRestResponse Response {
            get {
                return _response;
            }
        }
    }
}
