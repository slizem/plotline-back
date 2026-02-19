namespace Plotline.Application.Services;

using Plotline.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Интерфейс сервиса для работы с пользователями.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Пользователь или исключение NotFoundException</returns>
    Task<UserDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Коллекция пользователей</returns>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>
    /// Создать нового пользователя.
    /// </summary>
    /// <param name="createUserDto">Данные для создания пользователя</param>
    /// <returns>Созданный пользователь</returns>
    /// <exception cref="ConflictException">Если пользователь с таким email уже существует</exception>
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);

    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <param name="updateUserDto">Данные для обновления</param>
    /// <returns>Обновленный пользователь</returns>
    /// <exception cref="NotFoundException">Если пользователь не найден</exception>
    /// <exception cref="ConflictException">Если новый email уже занят</exception>
    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);

    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <exception cref="NotFoundException">Если пользователь не найден</exception>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Проверить учетные данные пользователя.
    /// </summary>
    /// <param name="email">Email пользователя</param>
    /// <param name="password">Пароль</param>
    /// <returns>True если учетные данные верные, иначе False</returns>
    Task<bool> ValidateCredentialsAsync(string email, string password);

    /// <summary>
    /// Получить пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя</param>
    /// <returns>Пользователь или исключение NotFoundException</returns>
    Task<UserDto> GetByEmailAsync(string email);
}