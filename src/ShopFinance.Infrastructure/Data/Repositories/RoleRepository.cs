using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopFinance.Domain.Entities;
using ShopFinance.Domain.Repositories;
using System.Security.Claims;

namespace ShopFinance.Infrastructure.Data.Repositories;

internal class RoleRepository : Repository<Role, Guid>, IRoleRepository
{
    private readonly RoleManager<Role> _roleManager;

    public RoleRepository(
        ApplicationDbContext context,
        RoleManager<Role> roleManager) : base(context)
    {
        _roleManager = roleManager;
    }

    public async Task<bool> ExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<List<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        // Asumiendo que Role tiene una propiedad IsActive
        // Si no la tiene, puedes ajustar según tu entidad
        return await _context.Roles
            .Where(r => r.IsActive) // Ajusta según tu entidad Role
            .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _roleManager.FindByNameAsync(roleName);
    }

    public async Task<bool> IsNameUniqueAsync(string roleName, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Roles.Where(r => r.Name == roleName);

        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }



    // Métodos adicionales útiles para gestión de roles
    public async Task<IdentityResult> CreateWithClaimsAsync(Role role, IEnumerable<Claim> claims)
    {
        var result = await _roleManager.CreateAsync(role);
        if (result.Succeeded && claims != null)
        {
            foreach (var claim in claims)
            {
                await _roleManager.AddClaimAsync(role, claim);
            }
        }
        return result;
    }

    public async Task<IList<Claim>> GetClaimsAsync(Guid roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        return role != null ? await _roleManager.GetClaimsAsync(role) : new List<Claim>();
    }

    public async Task<IdentityResult> AddClaimAsync(Guid roleId, Claim claim)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        return role != null
            ? await _roleManager.AddClaimAsync(role, claim)
            : IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });
    }

    public async Task<IdentityResult> RemoveClaimAsync(Guid roleId, Claim claim)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        return role != null
            ? await _roleManager.RemoveClaimAsync(role, claim)
            : IdentityResult.Failed(new IdentityError { Description = "Rol no encontrado" });
    }

    // Métodos usando Specification
    public async Task<List<Role>> GetRolesBySpecAsync(ISpecification<Role> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetRoleBySpecAsync(ISpecification<Role> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }
}