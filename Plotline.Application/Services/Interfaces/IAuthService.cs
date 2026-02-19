using Plotline.Application.DTOs.Auth;

namespace Plotline.Application.Services
{
    /// <summary>
    /// Сервис для работы с аутентификацией.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="registerDto">Данные для регистрации.</param>
        /// <returns>Токены и данные пользователя.</returns>
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Вход в систему.
        /// </summary>
        /// <param name="dto">Данные для входа.</param>
        /// <param name="ipAddress">IP-адрес клиента.</param>
        /// <param name="userAgent">User-Agent клиента.</param>
        /// <returns>Токены и данные пользователя.</returns>
        Task<AuthResponseDto> LoginAsync(LoginDto dto, string? ipAddress, string? userAgent);

        /// <summary>
        /// Обновление токенов.
        /// </summary>
        /// <param name="refreshToken">Refresh токен.</param>
        /// <param name="ipAddress">IP-адрес клиента.</param>
        /// <param name="userAgent">User-Agent клиента.</param>
        /// <returns>Новые токены и данные пользователя.</returns>
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress, string? userAgent);
    }
}