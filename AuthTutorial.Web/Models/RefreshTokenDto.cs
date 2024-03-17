using System.ComponentModel.DataAnnotations;

namespace Casino.Admin.Contracts
{
    public record RefreshTokenDto
    {
        /// <summary>
        /// Токен обновления доступа
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
