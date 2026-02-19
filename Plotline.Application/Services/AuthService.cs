namespace Plotline.Application.Services;

using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Plotline.Application.DTOs;
using Plotline.Application.DTOs.Auth;
using Plotline.Application.Exceptions;
using Plotline.Core.Interfaces;
using Plotline.Core.Interfaces.Repositories;
using Plotline.Core.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Сервис аутентификации и работы с токенами.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор сервиса аутентификации.
    /// </summary>
    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IOptions<JwtSettings> jwtSettings,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtSettings.Value;
        _mapper = mapper;
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.ExistsByLoginAsync(registerDto.Login))
        {
            throw new ConflictException($"Login '{registerDto.Login}' is already taken");
        }

        if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
        {
            throw new ConflictException($"Email '{registerDto.Email}' is already registered");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Login = registerDto.Login,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName ?? registerDto.Login,
            PasswordHash = _passwordHasher.Hash(registerDto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            AccessToken = token,
            User = _mapper.Map<UserDto>(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
        };
    }

    /// <summary>
    /// Вход в систему.
    /// </summary>
    public async Task<AuthResponseDto> LoginAsync(
        LoginDto loginDto,
        string? ipAddress = null,
        string? userAgent = null)
    {
        var user = await _userRepository.GetByLoginAsync(loginDto.Login);

        if (user == null || !_passwordHasher.Verify(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid login or password");

        if (!user.IsActive)
            throw new UnauthorizedException("Account is deactivated");

        await _refreshTokenRepository.RevokeForUserAsync(user.Id);

        var accessToken = GenerateJwtToken(user);
        var plainToken = GenerateRandomToken();
        var tokenHash = _passwordHasher.Hash(plainToken);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = tokenHash,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            IsRevoked = false,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _refreshTokenRepository.AddAsync(refreshToken);
        await _userRepository.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = plainToken,
            User = _mapper.Map<UserDto>(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
        };
    }

    /// <summary>
    /// Генерация JWT токена доступа.
    /// </summary>
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("display_name", user.DisplayName ?? user.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Обновление токенов.
    /// </summary>
    public async Task<AuthResponseDto> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        string? userAgent = null)
    {
        var storedToken = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);

        if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedException("Invalid refresh token");

        var user = await _userRepository.GetByIdAsync(storedToken.UserId);

        if (user == null || !user.IsActive)
            throw new UnauthorizedException("User not found or inactive");

        storedToken.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(storedToken);

        var newPlainToken = GenerateRandomToken();
        var newTokenHash = _passwordHasher.Hash(newPlainToken);

        var newRefreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = newTokenHash,
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(1),
            IsRevoked = false,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
        await _userRepository.SaveChangesAsync();

        var newAccessToken = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newPlainToken,
            User = _mapper.Map<UserDto>(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes)
        };
    }

    /// <summary>
    /// Генерация случайного токена.
    /// </summary>
    private string GenerateRandomToken()
    {
        var randomBytes = new byte[32];
        RandomNumberGenerator.Fill(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}