using Microsoft.AspNetCore.Identity;
using ShopFinance.Domain.Entities;

namespace ShopFinance.Application.Features.Users.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UserManager<User> _userManager;

    public CreateUserCommandValidator(UserManager<User> userManager)
    {
        _userManager = userManager;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("El nombre de usuario es requerido")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
            .MaximumLength(256).WithMessage("El nombre de usuario no puede exceder 256 caracteres")
            .Matches(@"^[a-zA-Z0-9_.-]+$").WithMessage("El nombre de usuario solo puede contener letras, números, puntos, guiones y guiones bajos")
            .MustAsync(BeUniqueUserName).WithMessage("El nombre de usuario ya está en uso");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El formato del email no es válido")
            .MaximumLength(256).WithMessage("El email no puede exceder 256 caracteres")
            .MustAsync(BeUniqueEmail).WithMessage("El email ya está registrado");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("El nombre completo es requerido")
            .MaximumLength(256).WithMessage("El nombre completo no puede exceder 256 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MustAsync(ValidatePasswordAsync).WithMessage("La contraseña no cumple con los requisitos de seguridad");

        RuleFor(x => x.PasswordConfirm)
            .Equal(x => x.Password).WithMessage("Las contraseñas no coinciden");

        RuleFor(x => x.AvatarUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.AvatarUrl))
            .WithMessage("La URL del avatar no es válida");
    }

    private async Task<bool> BeUniqueUserName(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user == null;
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null;
    }

    private async Task<bool> ValidatePasswordAsync(CreateUserCommand command, string password, CancellationToken cancellationToken)
    {
        // Crear un usuario temporal para validar la contraseña
        var user = new User
        {
            UserName = command.UserName,
            Email = command.Email
        };

        var result = await _userManager.CreateAsync(user, password);

        // Si la creación falla solo por la contraseña, retornar false
        if (!result.Succeeded && result.Errors.Any(e => e.Code.Contains("Password")))
        {
            return false;
        }

        // Si se creó exitosamente, eliminar el usuario temporal
        if (result.Succeeded)
        {
            await _userManager.DeleteAsync(user);
        }

        return result.Succeeded;
    }

    private bool BeValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out _) &&
               (url.StartsWith("http://") || url.StartsWith("https://"));
    }
}