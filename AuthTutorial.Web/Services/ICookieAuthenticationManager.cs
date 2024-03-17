using System.Security.Claims;
using AuthTutorial.Models;

namespace AuthTutorial.Services;

public interface ICookieAuthenticationManager
{
    Task LoginAsync(string login, string password);

    ClaimsPrincipal GetAuthorizedUser(TokensPairDto tokensPairDto);
}