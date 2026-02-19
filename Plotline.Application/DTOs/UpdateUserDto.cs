namespace Plotline.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateUserDto
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [MaxLength(100)]
    public string? Login { get; set; }

    /// <summary>
    /// Email пользователя.
    /// </summary>
    [EmailAddress]
    public string? Email { get; set; }
}