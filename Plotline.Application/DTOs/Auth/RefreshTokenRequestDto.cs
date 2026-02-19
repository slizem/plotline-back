using System.ComponentModel.DataAnnotations;

namespace Plotline.Application.DTOs.Auth
{
    /// <summary>
    /// DTO токена обновления.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        /// <summary>
        /// Токен обновления.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}