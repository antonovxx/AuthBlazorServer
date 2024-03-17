using System.IdentityModel.Tokens.Jwt;
using AuthTutorial.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthTutorial.Services;

public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
{
    private const string ACCESS_TOKEN = "access_token";
    private const string REFRESH_TOKEN = "refresh_token";
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAdminAuthHttpCLient _ssoClient;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly ICookieAuthenticationManager _cookieAuthenticationManager;

    public CustomCookieAuthenticationEvents(
        IHttpContextAccessor httpContextAccessor,
        IAdminAuthHttpCLient ssoClient,
        ICookieAuthenticationManager cookieAuthenticationManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _ssoClient = ssoClient;
        _cookieAuthenticationManager = cookieAuthenticationManager;
    }

    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var userPrincipal = context.Principal;

        var accessToken = userPrincipal.Claims.FirstOrDefault(x => x.Type == ACCESS_TOKEN).Value;
        var refreshToken = userPrincipal.Claims.FirstOrDefault(x => x.Type == REFRESH_TOKEN).Value;
        
        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
        {
            return;
        }
        
        var accessTokenJwt = _tokenHandler.ReadJwtToken(accessToken);
        
        if (accessTokenJwt.ValidTo < DateTime.UtcNow)
        {
            var newTokensPair = await _ssoClient.RefreshAsync(refreshToken);
        
            if (newTokensPair is null)
            {
                return;
            }
        
            var newUser = _cookieAuthenticationManager.GetAuthorizedUser(newTokensPair);
            await _httpContextAccessor.HttpContext.SignInAsync(newUser);
        }
    }
}