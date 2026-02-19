namespace Plotline.Core.Interfaces.Repositories;

using Plotline.Core.Models;
using System.Threading.Tasks;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task<int> GetCountAsync();
    Task<User> GetByLoginAsync(string login);
    Task<bool> ExistsByLoginAsync(string login);
}