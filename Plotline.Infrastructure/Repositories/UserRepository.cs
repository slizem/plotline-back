namespace Plotline.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Plotline.Core.Interfaces.Repositories;
using Plotline.Core.Models;
using Plotline.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Login)
            .ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        if (user.Id == Guid.Empty)
            user.Id = Guid.NewGuid();

        if (user.CreatedAt == default)
            user.CreatedAt = DateTime.UtcNow;

        await _context.Users.AddAsync(user);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser == null)
            throw new InvalidOperationException($"User with id {user.Id} not found");

        existingUser.Login = user.Login;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash;
        existingUser.UpdatedAt = DateTime.UtcNow;

        _context.Entry(existingUser).State = EntityState.Modified;
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Users
            .AnyAsync(u => u.Id == id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Users.CountAsync();
    }

    public async Task<User> GetByLoginAsync(string login)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());
    }

    public async Task<bool> ExistsByLoginAsync(string login)
    {
        return await _context.Users
            .AnyAsync(u => u.Login.ToLower() == login.ToLower());
    }
}