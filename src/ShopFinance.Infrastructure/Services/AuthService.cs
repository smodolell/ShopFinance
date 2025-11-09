//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using ShopFinance.Application.Common.Interfaces;
//using ShopFinance.Application.Features.Auth.DTOs;
//using ShopFinance.Domain.Entities;
//using System.Security.Claims;

//namespace ShopFinance.Infrastructure.Services;

//public class AuthService : IAuthService
//{
//    private readonly SignInManager<User> _signInManager;
//    private readonly UserManager<User> _userManager;
//    private readonly ILogger<AuthService> _logger;
//    private readonly IJwtHelper _jwtHelper;

//    public AuthService(
//        SignInManager<User> signInManager,
//        UserManager<User> userManager,
//        ILogger<AuthService> logger,
//        IJwtHelper jwtHelper) // ← Nueva dependencia
//    {
//        _signInManager = signInManager;
//        _userManager = userManager;
//        _logger = logger;
//        _jwtHelper = jwtHelper;
//    }

//    public async Task<LoginResponse> LoginAsync(LoginRequest request)
//    {
//        try
//        {
//            var user = await _userManager.FindByEmailAsync(request.Email);
//            if (user == null || !user.IsActive)
//            {
//                _logger.LogWarning("Login fallido: usuario no encontrado {Email}", request.Email);
//                return new LoginResponse(false, "Credenciales inválidas");
//            }

//            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

//            if (!isValidPassword)
//            {
//                _logger.LogWarning("Login fallido: contraseña incorrecta para {Email}", request.Email);

//                // Incrementar contador de intentos fallidos
//                await _userManager.AccessFailedAsync(user);

//                // Verificar si está bloqueado después del intento fallido
//                if (await _userManager.IsLockedOutAsync(user))
//                {
//                    return new LoginResponse(false, "Cuenta bloqueada temporalmente por múltiples intentos fallidos");
//                }

//                return new LoginResponse(false, "Credenciales inválidas");
//            }

//            // ✅ RESETEAR CONTADOR DE FALLOS si la contraseña es correcta
//            await _userManager.ResetAccessFailedCountAsync(user);

//            var roles = await _userManager.GetRolesAsync(user);

//            // ✅ GENERAR TOKEN JWT
//            var token = await GenerateJwtTokenAsync(user, roles);

//            _logger.LogInformation("Login exitoso: {Email}", request.Email);

//            return new LoginResponse(
//                true,
//                "Login exitoso",
//                "/",
//                new UserDto(
//                    user.Id,
//                    user.Email ?? "",
//                    user.FirstName,
//                    user.LastName,
//                    $"{user.FirstName} {user.LastName}",
//                    roles.ToList()),
//                token);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error durante el proceso de login para {Email}", request.Email);
//            return new LoginResponse(false, "Error interno del servidor");
//        }
//    }
//    private async Task<string> GenerateJwtTokenAsync(User user, IList<string> roles)
//    {
//        var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//            new Claim(ClaimTypes.Email, user.Email ?? ""),
//            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
//            new Claim("full_name", $"{user.FirstName} {user.LastName}"),
//            new Claim("first_name", user.FirstName),
//            new Claim("last_name", user.LastName),
//            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//        };

//        // Agregar roles como claims
//        foreach (var role in roles)
//        {
//            claims.Add(new Claim(ClaimTypes.Role, role));
//        }

//        // Agregar claims personalizados desde el usuario
//        var userClaims = await _userManager.GetClaimsAsync(user);
//        claims.AddRange(userClaims);

//        // Generar token JWT
//        var token = await _jwtHelper.GenerateJwtToken(claims);

//        return token;
//    }

//    public async Task LogoutAsync()
//    {
//        await _signInManager.SignOutAsync();
//    }

//    public async Task<UserDto> GetCurrentUserAsync()
//    {
//        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
//        if (user == null) return null;

//        var roles = await _userManager.GetRolesAsync(user);
//        return new UserDto(
//            user.Id,
//            user.Email ?? "",
//            user.FirstName,
//            user.LastName,
//            $"{user.FirstName} {user.LastName}",
//            roles.ToList());
//    }

//    public Task<bool> IsUserAuthenticatedAsync()
//    {
//        return Task.FromResult(_signInManager.Context.User.Identity?.IsAuthenticated ?? false);
//    }

//    // Método adicional para refrescar token
//    public async Task<string?> RefreshTokenAsync(string userId)
//    {
//        var user = await _userManager.FindByIdAsync(userId);
//        if (user == null || !user.IsActive) return null;

//        var roles = await _userManager.GetRolesAsync(user);
//        return await GenerateJwtTokenAsync(user, roles);
//    }
//}