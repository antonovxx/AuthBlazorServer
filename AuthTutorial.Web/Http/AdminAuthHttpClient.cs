using AuthTutorial.Models;
using Casino.Admin.Contracts;

namespace AuthTutorial.Http
{
    /// <inheritdoc/>
    public class AdminAuthHttpCLient : HttpClientBase, IAdminAuthHttpCLient
    {
        private const string LOGIN_URL = "admin/login";

        private const string REFRESH_URL = "admin/refresh";

        private const string LOGOUT_URL = "admin/logout";

        public AdminAuthHttpCLient(HttpClient client, ILogger<AdminAuthHttpCLient> logger) : base(client, logger)
        {
        }

        /// <inheritdoc/>
        public Task<TokensPairDto> LoginAsync(string login, string password)
        {
            return PostAsync<TokensPairDto>(
                LOGIN_URL,
                new LoginDto
                {
                    Login = login,
                    Password = password
                });
        }

        /// <inheritdoc/>
        public Task<TokensPairDto> RefreshAsync(string refreshToken)
        {
            return PostAsync<TokensPairDto>(
                REFRESH_URL,
                new RefreshTokenDto { RefreshToken = refreshToken });
        }

        /// <inheritdoc/>
        public Task LogoutAsync(string refreshToken)
        {
            return PostAsync(
                LOGOUT_URL,
                new RefreshTokenDto { RefreshToken = refreshToken });
        }
    }
}
