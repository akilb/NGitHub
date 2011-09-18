using RestSharp;

namespace NGitHub.Authentication {
    public class NullAuthenticator : IAuthenticator {
        public void Authenticate(RestClient client, RestRequest request) {
            // NOOP
        }
    }
}