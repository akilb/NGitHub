using System;
using NGitHub.Models;

namespace NGitHub {
    public interface IUserService {
        void GetUserAsync(string user, Action<User> callback, Action<APICallError> onError);
    }
}
