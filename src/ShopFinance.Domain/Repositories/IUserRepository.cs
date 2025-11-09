using Ardalis.Result;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Domain.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
    Task<Result> CheckPasswordAsync(Guid userId, string password);
    Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<Result> CreateAsync(User user, string password);


}
