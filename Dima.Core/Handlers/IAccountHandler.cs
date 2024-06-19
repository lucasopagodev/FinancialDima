using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Core;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}
