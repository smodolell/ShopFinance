using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Infrastructure.Data.Initializers;

internal class UserInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public UserInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string wbmasterRoleName = "Webmaster";
            bool adminRoleExists = await roleManager.RoleExistsAsync(wbmasterRoleName);

            if (!adminRoleExists)
            {
                var role = Role.Create(wbmasterRoleName, "Administrador de la aplicacion");
                role.Activate();
                await roleManager.CreateAsync(role);
            }

            string adminEmail = "sergio.modolell@gmail.com";
            string adminPassword = "Sergio123456%"; // En producción usa una contraseña segura

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = User.Create(Guid.NewGuid(),
                    adminEmail,
                    adminEmail,
                    "Sergio",
                    "Modolell");
           

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (createResult.Succeeded)
                {
                    // Asignar rol al usuario
                    await userManager.AddToRoleAsync(adminUser, wbmasterRoleName);
                }
                else
                {
                    // Manejar errores de creación
                    var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception($"No se pudo crear el usuario administrador: {errors}");
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
