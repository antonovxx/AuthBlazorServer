using AuthTutorial.Models;

namespace AuthTutorial.Http
{
    public interface IAdminAuthHttpCLient
    {
        /// <summary>
        /// Войти в систем
        /// </summary>
        /// <param name="login">Логину</param>
        /// <param name="password">Паролю</param>
        /// <returns>Токены доступа и обновления</returns>
        Task<TokensPairDto> LoginAsync(string login, string password);

        /// <summary>
        /// Обновить токены доступа
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <returns>Новая пара токенов доступа</returns>
        Task<TokensPairDto> RefreshAsync(string refreshToken);

        /// <summary>
        /// Выйти из системы
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <returns></returns>
        Task LogoutAsync(string refreshToken);
    }
}
