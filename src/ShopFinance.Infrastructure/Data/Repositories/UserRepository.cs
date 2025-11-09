using Ardalis.Result;
using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;

namespace ShopFinance.Infrastructure.Data.Repositories;

internal class UserRepository : Repository<User, Guid>, IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UserRepository(
        ApplicationDbContext context,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
        : base(context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<Result> CheckPasswordAsync(Guid userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound($"Usuario con ID {userId} no encontrado");

        if (!user.IsActive)
            return Result.Forbidden("Usuario inactivo");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

        return result.Succeeded
            ? Result.Success()
            : Result.Unauthorized("Credenciales inválidas");
    }

    public async Task<Result> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound($"Usuario con ID {userId} no encontrado");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.Select(e => e.Description);
        return Result.Error(new ErrorList(errors.ToArray()));
    }

    public async Task<Result> CreateAsync(User user, string password)
    {
        // Asegurar que el UserName esté establecido si es requerido
        if (string.IsNullOrEmpty(user.UserName))
            user.UserName = user.Email;

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.Select(e =>  e.Description);
        return Result.Error(new ErrorList(errors.ToArray()));
    }


    // Métodos adicionales útiles para gestión de usuarios
    public async Task<Result> AddToRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound($"Usuario con ID {userId} no encontrado");

        var result = await _userManager.AddToRoleAsync(user, role);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.Select(e => e.Description);
        return Result.Error(new ErrorList(errors.ToArray()));
    }

    public async Task<Result> RemoveFromRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound($"Usuario con ID {userId} no encontrado");

        var result = await _userManager.RemoveFromRoleAsync(user, role);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.Select(e => e.Description);
        return Result.Error(new ErrorList(errors.ToArray()));
    }

    public async Task<IList<string>> GetRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null
            ? await _userManager.GetRolesAsync(user)
            : new List<string>();
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<Result> ResetPasswordAsync(Guid userId, string token, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return Result.NotFound($"Usuario con ID {userId} no encontrado");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (result.Succeeded)
            return Result.Success();

        var errors = result.Errors.Select(e => e.Description);
        return Result.Error(new ErrorList(errors.ToArray()));
    }

    public async Task<string> GeneratePasswordResetTokenAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user != null
            ? await _userManager.GeneratePasswordResetTokenAsync(user)
            : string.Empty;
    }


}