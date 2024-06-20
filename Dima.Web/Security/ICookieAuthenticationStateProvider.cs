using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public interface ICookieAuthenticationStateProvider
{
    Task<bool> CheckedAuthenticatedAsync();
    Task<AuthenticationState> GetAuthenticationStateAsync();
    void NotifyAuthenticationStateChanged();
}
