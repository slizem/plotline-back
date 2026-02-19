using Microsoft.EntityFrameworkCore;
using Plotline.Core.Interfaces;
using Plotline.Core.Interfaces.Repositories;
using Plotline.Core.Models;
using Plotline.Infrastructure.Data;

namespace Plotline.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public RefreshTokenRepository(ApplicationDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            var allTokens = await _context.RefreshTokens
                .Include(rt => rt.User)
                .Where(rt => !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            return allTokens.FirstOrDefault(rt =>
                _passwordHasher.Verify(refreshToken, rt.TokenHash));
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await Task.CompletedTask;
        }

        public async Task RevokeForUserAsync(Guid userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveExpiredAsync()
        {
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(expiredTokens);
        }
    }
}