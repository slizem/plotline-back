using Plotline.Core.Models;

namespace Plotline.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByRefreshTokenAsync(string tokenHash);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(Guid userId);
        Task AddAsync(RefreshToken token);
        Task UpdateAsync(RefreshToken token);
        Task RevokeForUserAsync(Guid userId);
        Task RemoveExpiredAsync();
    }
}