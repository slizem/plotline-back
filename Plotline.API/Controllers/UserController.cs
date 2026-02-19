namespace Plotline.API.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plotline.Application.DTOs;
using Plotline.Application.Services;
using System;
using System.Threading.Tasks;

/// <summary>
/// Контроллер для управления пользователями.
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary>
    /// Конструктор контроллера пользователей.
    /// </summary>
    /// <param name="userService">Сервис работы с пользователями</param>
    /// <exception cref="ArgumentNullException">Если userService равен null</exception>
    public UserController(IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// Получение пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя (GUID)</param>
    /// <returns>Данные пользователя</returns>
    /// <response code="200">Пользователь найден</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пользователь не найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    /// <summary>
    /// Получение списка всех пользователей.
    /// </summary>
    /// <returns>Список пользователей</returns>
    /// <response code="200">Список пользователей получен</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пользователи не найдены</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <param name="createUserDto">Данные для создания пользователя</param>
    /// <returns>Созданный пользователь</returns>
    /// <response code="201">Пользователь успешно создан</response>
    /// <response code="400">Некорректные данные</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="409">Конфликт данных (логин/email уже существует)</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Create(CreateUserDto createUserDto)
    {
        var user = await _userService.CreateAsync(createUserDto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя (GUID)</param>
    /// <param name="updateUserDto">Данные для обновления</param>
    /// <returns>Обновлённый пользователь</returns>
    /// <response code="200">Пользователь успешно обновлён</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пользователь не найден</response>
    /// <response code="409">Конфликт данных (логин/email уже существует)</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserDto>> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _userService.UpdateAsync(id, updateUserDto);
        return Ok(user);
    }

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя (GUID)</param>
    /// <returns>Статус 204 No Content</returns>
    /// <response code="204">Пользователь успешно удалён</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Пользователь не найден</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}