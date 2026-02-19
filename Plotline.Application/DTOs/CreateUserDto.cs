namespace Plotline.Application.DTOs;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO для создания пользователя.
/// </summary>
public class CreateUserDto
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
    [MinLength(6)]
    public string Password { get; set; }
}