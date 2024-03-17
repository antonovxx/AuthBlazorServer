using System.Text.Json.Serialization;

namespace AuthTutorial.Models
{
    public record TokensPairDto
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Момент истечения срока годности токена доступа
        /// </summary>
        [JsonPropertyName("expires_at_utc")]
        public DateTime ExpiresAtUtc { get; set; }

        /// <summary>
        /// Токен обновления токена доступа
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Момент истечения срока годности токена обновления
        /// </summary>
        [JsonPropertyName("refresh_expires_at_utc")]
        public DateTime RefreshExpiresAtUtc { get; set; }
    }
}
