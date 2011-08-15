using System;
using System.Net;
using NGitHub.Utility;
using RestSharp;

namespace NGitHub.Web {
    public abstract class GitHubResponseBase {
        private readonly IRestResponse _response;

        public GitHubResponseBase(IRestResponse response) {
            Requires.ArgumentNotNull(response, "response");

            _response = response;
        }

        public HttpStatusCode StatusCode {
            get {
                return _response.StatusCode;
            }
        }

        public ResponseStatus ResponseStatus {
            get {
                return _response.ResponseStatus.ToNGitHubResponseStatus();
            }
        }

        public bool IsError {
            get {
                return _response.StatusCode != HttpStatusCode.OK &&
                       _response.StatusCode != HttpStatusCode.Created;
            }
        }

        public string Content {
            get {
                return _response.Content;
            }
        }

        public string ContentType {
            get {
                return _response.ContentType;
            }
        }

        public string ErrorMessage {
            get {
                return _response.ErrorMessage;
            }
        }

        public Exception ErrorException {
            get {
                return _response.ErrorException;
            }
        }
    }
}
