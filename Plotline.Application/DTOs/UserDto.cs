namespace Plotline.Application.DTOs;

public class UserDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Создан (дата).
    /// </summary>
    public DateTime CreatedAt { get; set; }
}