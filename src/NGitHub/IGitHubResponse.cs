using System;
using System.Net;
using NGitHub.Web;

namespace NGitHub {
    public interface IGitHubResponse<T> : IGitHubResponse {
        T Data { get; }
    }

    public interface IGitHubResponse {
        string Content                  { get; }
        string ErrorMessage             { get; }
        Exception ErrorException        { get; }
        HttpStatusCode StatusCode       { get; }
        ResponseStatus ResponseStatus   { get; }
    }
}
