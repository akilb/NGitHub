using RestSharp;

namespace NGitHub.Test.Helpers {
    public static class TestHelpers {
        public static GitHubRequestAsyncHandle CreateTestHandle() {
            return new GitHubRequestAsyncHandle(new GitHubRequest("foo", API.v3, NGitHub.Web.Method.GET),
                                                new RestRequestAsyncHandle());
        }
    }
}
