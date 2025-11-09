using Microsoft.IdentityModel.Tokens;
using ShopFinance.Application.Common.Models.Configurations;
namespace ShopFinance.Application.Common.Interfaces;



public interface IJwtConfigurationProvider
{
    /// <summary>
    /// Obtiene los parámetros de validación de token JWT
    /// </summary>
    Task<TokenValidationParameters> GetTokenValidationParametersAsync();

    /// <summary>
    /// Obtiene la clave para firmar tokens (usa private key)
    /// </summary>
    Task<RsaSecurityKey> GetSigningKeyAsync();

    /// <summary>
    /// Obtiene la clave para validar tokens (usa public key)
    /// </summary>
    Task<RsaSecurityKey> GetValidationKeyAsync();

    /// <summary>
    /// Obtiene el emisor del token
    /// </summary>
    Task<string> GetIssuerAsync();

    /// <summary>
    /// Obtiene la audiencia del token
    /// </summary>
    Task<string> GetAudienceAsync();

    /// <summary>
    /// Obtiene el tiempo de expiración en minutos
    /// </summary>
    Task<int> GetExpireMinutesAsync();

    /// <summary>
    /// Obtiene la configuración completa de JWT
    /// </summary>
    Task<JwtConfig> GetJwtConfigAsync();

    /// <summary>
    /// Refresca la caché de configuración
    /// </summary>
    Task RefreshCacheAsync();
}

