using Microsoft.AspNetCore.Http;
using Plotline.Core.Interfaces.Services;

namespace Plotline.Application.Services
{
    /// <summary>
    /// Сервис для получения информации о клиенте из HTTP-запроса.
    /// </summary>
    public class ClientInfoService : IClientInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Конструктор сервиса.
        /// </summary>
        /// <param name="httpContextAccessor">Доступ к HTTP-контексту</param>
        public ClientInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Получить IP-адрес клиента.
        /// </summary>
        /// <returns>IP-адрес в виде строки</returns>
        public string GetClientIp() =>
            _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        /// <summary>
        /// Получить User-Agent клиента.
        /// </summary>
        /// <returns>User-Agent строка</returns>
        public string GetUserAgent() =>
            _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
    }
}