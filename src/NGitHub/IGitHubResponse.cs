using System.Net;

namespace NGitHub {
    public interface IGitHubResponse<T> : IGitHubResponse {
        T Data { get; }
    }

    public interface IGitHubResponse {
        bool IsError { get; }
        HttpStatusCode StatusCode { get; }
    }
}
