using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plotline.Core.Models
{
    /// <summary>
    /// Таблица пользователей.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Шифрованный пароль.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Отображаемое имя.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Активность учетной записи.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Дата редактирования.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Создал.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Изменил.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

