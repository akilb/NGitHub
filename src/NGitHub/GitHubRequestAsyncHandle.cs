using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using NGitHub.Utility;

namespace NGitHub {
    public class GitHubRequestAsyncHandle {
        private readonly GitHubRequest _request;
        private readonly RestRequestAsyncHandle _handle;

        public GitHubRequestAsyncHandle(GitHubRequest request,
                                        RestRequestAsyncHandle handle) {
            Requires.ArgumentNotNull(request, "request");
            Requires.ArgumentNotNull(handle, "handle");

            _request = request;
            _handle = handle;
        }

        public GitHubRequest Request {
            get {
                return _request;
            }
        }

        public void Abort() {
            _handle.Abort();
        }
    }
}
