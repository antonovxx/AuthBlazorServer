using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthTutorial.Http;
using AuthTutorial.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthTutorial.Services;

public class CookieAuthenticationManager : ICookieAuthenticationManager
{
    private const string ACCESS_TOKEN = "access_token";
    private const string REFRESH_TOKEN = "refresh_token";
    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAdminAuthHttpCLient _ssoClient;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    public CookieAuthenticationManager(IHttpContextAccessor httpContextAccessor, IAdminAuthHttpCLient ssoClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _ssoClient = ssoClient;
    }

    public async Task LoginAsync(string login, string password)
    {
        var tokensPair = await _ssoClient.LoginAsync(login, password);

        if (tokensPair is null)
        {
            throw new ArgumentException("Tokens pair is null");
        }

        var context = _httpContextAccessor.HttpContext;

        var user = GetAuthorizedUser(tokensPair);

        await context.SignInAsync(user);
    }


    public ClaimsPrincipal GetAuthorizedUser(TokensPairDto tokensPairDto)
    {
        var accessToken = _tokenHandler.ReadJwtToken(tokensPairDto.AccessToken);

        var claims = accessToken.Claims
            .Where(claim => claim.Type == "roles")
            .Select(claim => new Claim(ClaimTypes.Role, claim.Value))
            .ToList();
        
        claims.Add(new Claim(ACCESS_TOKEN, tokensPairDto.AccessToken));
        claims.Add(new Claim(REFRESH_TOKEN, tokensPairDto.RefreshToken));

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(identity);
    }
}