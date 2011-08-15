using System;

namespace NGitHub {
    internal static class Extensions {
        public static RestSharp.Method ToRestSharpMethod(this Method method) {
            switch (method) {
                case Method.GET:
                    return RestSharp.Method.GET;
                case Method.POST:
                    return RestSharp.Method.POST;
                case Method.PUT:
                    return RestSharp.Method.PUT;
                case Method.DELETE:
                    return RestSharp.Method.DELETE;
                case Method.HEAD:
                    return RestSharp.Method.HEAD;
                case Method.OPTIONS:
                    return RestSharp.Method.OPTIONS;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static ResponseStatus ToNGitHubResponseStatus(this RestSharp.ResponseStatus status) {
            switch (status) {
                case RestSharp.ResponseStatus.Completed:
                    return ResponseStatus.Completed;
                case RestSharp.ResponseStatus.Error:
                    return ResponseStatus.Error;
                case RestSharp.ResponseStatus.None:
                    return ResponseStatus.None;
                case RestSharp.ResponseStatus.TimedOut:
                    return ResponseStatus.TimedOut;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
