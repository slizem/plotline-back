namespace Plotline.Application.DTOs.Auth
{
    /// <summary>
    /// Настройки JWT токенов.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Секретный ключ для подписи токенов.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Издатель токенов.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Получатель токенов.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Время жизни Access токена в минутах.
        /// </summary>
        public int ExpiresInMinutes { get; set; }

        /// <summary>
        /// Время жизни Refresh токена в днях.
        /// </summary>
        public int RefreshTokenExpiresInDays { get; set; }
    }
}