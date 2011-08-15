using System;
using System.Net;

namespace NGitHub {
    public interface IGitHubResponse<T> : IGitHubResponse {
        T Data { get; }
    }

    public interface IGitHubResponse {
        string ErrorMessage             { get; }
        Exception ErrorException        { get; }
        HttpStatusCode StatusCode       { get; }
        ResponseStatus ResponseStatus   { get; }
    }
}
