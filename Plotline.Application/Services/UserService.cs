// Plotline.Application/Services/UserService.cs
namespace Plotline.Application.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Plotline.Application.DTOs;
using Plotline.Application.Exceptions;
using Plotline.Core.Interfaces;
using Plotline.Core.Interfaces.Repositories;
using Plotline.Core.Models;

/// <summary>
/// Сервис для работы с пользователями
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Конструктор сервиса пользователей
    /// </summary>
    public UserService(
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<UserService> logger,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Получить пользователя по ID
    /// </summary>
    public async Task<UserDto> GetByIdAsync(Guid id)
    {
        _logger.LogDebug("Getting user by ID: {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        return _mapper.Map<UserDto>(user);
    }

    /// <summary>
    /// Получить всех пользователей
    /// </summary>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        _logger.LogDebug("Getting all users");
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        _logger.LogInformation("Creating user with email: {Email}", createUserDto.Email);

        if (await _userRepository.ExistsByEmailAsync(createUserDto.Email))
        {
            throw new ConflictException($"User with email '{createUserDto.Email}' already exists");
        }

        var user = _mapper.Map<User>(createUserDto);

        user.PasswordHash = _passwordHasher.Hash(createUserDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("User created with ID: {UserId}", user.Id);
        return _mapper.Map<UserDto>(user);
    }

    /// <summary>
    /// Обновить данные пользователя
    /// </summary>
    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        // Добавить проверку логина (помимо email) и DisplayName.

        // Проверка email только если он меняется
        if (!string.IsNullOrEmpty(updateUserDto.Email) &&
            updateUserDto.Email != user.Email)
        {
            if (await _userRepository.ExistsByEmailAsync(updateUserDto.Email))
            {
                throw new ConflictException($"Email '{updateUserDto.Email}' is already taken");
            }
        }

        _mapper.Map(updateUserDto, user);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        // Добавить мягкое удаление и каскадное удаление токенов.

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Проверить учетные данные пользователя
    /// </summary>
    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        _logger.LogDebug("Validating credentials for email: {Email}", email);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null) return false;

        return _passwordHasher.Verify(password, user.PasswordHash);
    }

    /// <summary>
    /// Получить пользователя по email
    /// </summary>
    public async Task<UserDto> GetByEmailAsync(string email)
    {
        _logger.LogDebug("Getting user by email: {Email}", email);

        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
        {
            throw new NotFoundException($"User with email '{email}' not found");
        }

        return _mapper.Map<UserDto>(user);
    }
}