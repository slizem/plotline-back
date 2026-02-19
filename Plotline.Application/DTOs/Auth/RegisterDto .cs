using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotline.Application.DTOs.Auth
{
    /// <summary>
    /// DTO для регистрации нового пользователя.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Login { get; set; }

        /// <summary>
        /// Email пользователя.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        /// <summary>
        /// Отображаемое имя пользователя.
        /// </summary>
        [MaxLength(255)]
        public string DisplayName { get; set; }
    }
}