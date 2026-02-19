using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plotline.Application.DTOs.Auth;
using Plotline.Application.Services;
using Plotline.Core.Interfaces.Services;
using System.Security.Claims;

namespace Plotline.API.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации и управления токенами.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClientInfoService _clientInfoService;

        /// <summary>
        /// Конструктор контроллера аутентификации.
        /// </summary>
        /// <param name="authService">Сервис аутентификации</param>
        /// <param name="clientInfoService">Сервис информации о клиенте</param>
        /// <exception cref="ArgumentNullException">Если authService равен null</exception>
        public AuthController(IAuthService authService, IClientInfoService clientInfoService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _clientInfoService = clientInfoService;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="dto">Данные для регистрации</param>
        /// <returns>JWT токен и данные пользователя</returns>
        /// <response code="200">Успешная регистрация</response>
        /// <response code="400">Некорректные данные</response>
        /// <response code="409">Пользователь с таким логином/email уже существует</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Вход в систему.
        /// </summary>
        /// <param name="dto">Данные для входа</param>
        /// <returns>JWT токен и данные пользователя</returns>
        /// <response code="200">Успешный вход</response>
        /// <response code="401">Неверный логин или пароль</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var ipAddress = _clientInfoService.GetClientIp();
            var userAgent = _clientInfoService.GetUserAgent();

            var result = await _authService.LoginAsync(dto, ipAddress, userAgent);
            return Ok(result);
        }

        /// <summary>
        /// Обновление токенов доступа.
        /// </summary>
        /// <param name="request">Запрос с refresh токеном</param>
        /// <returns>Новая пара токенов (access + refresh)</returns>
        /// <response code="200">Токены успешно обновлены</response>
        /// <response code="401">Невалидный или отозванный refresh токен</response>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var ipAddress = _clientInfoService.GetClientIp();
            var userAgent = _clientInfoService.GetUserAgent();

            var result = await _authService.RefreshTokenAsync(request.RefreshToken, ipAddress, userAgent);
            return Ok(result);
        }

        /// <summary>
        /// Выход из системы (отзыв токенов).
        /// </summary>
        /// <returns>Статус 204 No Content</returns>
        /// <response code="204">Успешный выход</response>
        /// <response code="401">Пользователь не авторизован</response>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userId, out var userGuid))
            {
                // Реализовать метод отзыва всех токенов пользователя.
                // await _authService.RevokeAllRefreshTokensForUserAsync(userGuid);
            }

            return NoContent();
        }
    }
}