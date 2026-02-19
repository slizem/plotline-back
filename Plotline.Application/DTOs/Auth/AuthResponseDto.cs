namespace Plotline.Application.DTOs.Auth
{
    /// <summary>
    /// Ответ аутентификации с токенами и данными пользователя.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT токен доступа.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Токен для обновления.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Данные пользователя.
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// Время истечения AccessToken.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}