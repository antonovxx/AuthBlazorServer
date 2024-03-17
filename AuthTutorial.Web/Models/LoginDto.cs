using System.ComponentModel.DataAnnotations;

namespace Casino.Admin.Contracts
{
    public record LoginDto
    {
        /// <summary>
        /// Логин
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
