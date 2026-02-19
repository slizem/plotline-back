using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotline.Application.DTOs.Auth
{
    /// <summary>
    /// DTO для входа в систему.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [Required(ErrorMessage = "Логин обязателен")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 50 символов")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [Required(ErrorMessage = "Пароль обязателен")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 30 символов")]
        public string Password { get; set; }
    }
}
