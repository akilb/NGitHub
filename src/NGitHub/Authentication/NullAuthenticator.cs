using RestSharp;

namespace NGitHub.Authentication {
    public class NullAuthenticator : IAuthenticator {
        public void Authenticate(IRestClient client, IRestRequest request) {
            // NOOP
        }
    }
}