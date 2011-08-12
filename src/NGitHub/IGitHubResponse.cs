using System.Net;

namespace NGitHub {
    public interface IGitHubResponse<T> {
        T Data { get; }
        bool IsError { get; }
        HttpStatusCode StatusCode { get; }
    }
}
