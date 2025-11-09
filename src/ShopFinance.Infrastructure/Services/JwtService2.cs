using BitzArt.Blazor.Auth;
using BitzArt.Blazor.Auth.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Domain.Entities;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopFinance.Infrastructure.Services;

public class JwtService2
{

    private readonly IJwtConfigurationProvider _jwtConfigProvider;

    public JwtService2(IJwtConfigurationProvider jwtConfigProvider)
    {
        _jwtConfigProvider = jwtConfigProvider;
    }

    public async Task<JwtPair> BuildJwtPair(List<Claim> claims)
    {
        var now = DateTime.UtcNow;
        var signingKey = await _jwtConfigProvider.GetSigningKeyAsync();
        var issuer = await _jwtConfigProvider.GetIssuerAsync();
        var audience = await _jwtConfigProvider.GetAudienceAsync();
        var accessTokenDuration = new TimeSpan(hours: 0, minutes: 15, seconds: 0);
        var accessTokenExpiresAt = now + accessTokenDuration;

        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = accessTokenExpiresAt,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = signingCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshTokenDuration = new TimeSpan(hours: 1, minutes: 0, seconds: 0);
        var refreshTokenExpiresAt = now + refreshTokenDuration;
        var refreshToken = tokenHandler.WriteToken(new JwtSecurityToken(
            notBefore: now,
            expires: refreshTokenExpiresAt,
            signingCredentials: signingCredentials
        ));

        return new JwtPair(accessToken, accessTokenExpiresAt, refreshToken, refreshTokenExpiresAt);
    }

    public async Task<ClaimsPrincipal?> ValidateToken(string token)
    {
        var signingKey = await _jwtConfigProvider.GetSigningKeyAsync();
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = signingCredentials.Key,
            ValidateIssuerSigningKey = true
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}

public class SignInPayload
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}
public class CustomAuthenticationService : AuthenticationService<SignInPayload>
{
    private readonly JwtService2 _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CustomAuthenticationService> _logger;

    public CustomAuthenticationService(JwtService2 jwtService, UserManager<User> userManager, ILogger<CustomAuthenticationService> logger)
    {
        _jwtService = jwtService;
        _userManager = userManager;
        _logger = logger;
    }

    public override async Task<AuthenticationResult> RefreshJwtPairAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var principal = await _jwtService.ValidateToken(refreshToken);
        if (principal == null)
            return Failure("Refresh token inválido o expirado");
        var username = principal.Identity?.Name ?? "";
        var user = await _userManager.FindByEmailAsync(username);

        if (user == null)
            return Failure("Usuario no encontrado");
        var roles = await _userManager.GetRolesAsync(user);

        var newJwtPair = await _jwtService.BuildJwtPair(GetClaim(user, roles.ToList()));

        return Success(newJwtPair);
    }

    public async override Task<AuthenticationResult> SignInAsync(SignInPayload signInPayload, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(signInPayload.Email);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login fallido: usuario no encontrado {Email}", signInPayload.Email);

                return Failure("Credenciales inválidas");
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, signInPayload.Password);

            if (!isValidPassword)
            {
                _logger.LogWarning("Login fallido: contraseña incorrecta para {Email}", signInPayload.Email);

                // Incrementar contador de intentos fallidos
                await _userManager.AccessFailedAsync(user);

                // Verificar si está bloqueado después del intento fallido
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return Failure("Cuenta bloqueada temporalmente por múltiples intentos fallidos");
                }

                return Failure("Credenciales inválidas");
            }

            // ✅ RESETEAR CONTADOR DE FALLOS si la contraseña es correcta
            await _userManager.ResetAccessFailedCountAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            // ✅ GENERAR TOKEN JWT
            var jwtPair = await _jwtService.BuildJwtPair(GetClaim(user, roles.ToList()));

            _logger.LogInformation("Login exitoso: {Email}", signInPayload.Email);

            var authResult = Success(jwtPair);

            return authResult;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el proceso de login para {Email}", signInPayload.Email);
            return Failure("Error interno del servidor");
        }
    }

    private List<Claim> GetClaim(User user, List<string> roles)
    {
        var now = DateTime.UtcNow;
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
        };

        // 🔹 Agregar roles (uno por cada rol asignado)
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
        => user.FindFirstValue("nameid") ?? "";

    public static string GetUserName(this ClaimsPrincipal user)
        => user.FindFirstValue("sub") ?? "";

    public static string GetUserEmail(this ClaimsPrincipal user)
        => user.FindFirstValue("email") ?? "";

    public static string GetFullName(this ClaimsPrincipal user)
    => user.FindFirstValue("unique_name") ?? "";

    public static IEnumerable<string> GetUserRoles(this ClaimsPrincipal user)
        => user.FindAll("role").Select(r => r.Value);


    public static string ToReadableString(this ClaimsPrincipal user)
    {
        if (user == null || !user.Identity?.IsAuthenticated == true)
            return "Usuario no autenticado";

        var sb = new StringBuilder();
        foreach (var claim in user.Claims)
        {
            sb.AppendLine($"{claim.Type}: {claim.Value}");
        }
        return sb.ToString();
    }
}