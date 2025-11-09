using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Application.Common.Models.Configurations;
using System.Security.Cryptography;

namespace ShopFinance.Infrastructure.Authentication;

public class JwtConfigurationProvider : IJwtConfigurationProvider
{
    private readonly IConfigurationService _configurationService;
    private readonly ILogger<JwtConfigurationProvider> _logger;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private JwtConfig? _cachedConfig;
    private DateTime _lastCacheUpdate = DateTime.MinValue;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(30);

    public JwtConfigurationProvider(
        IConfigurationService configurationService,
        ILogger<JwtConfigurationProvider> logger)
    {
        _configurationService = configurationService;
        _logger = logger;
    }

    public async Task<TokenValidationParameters> GetTokenValidationParametersAsync()
    {
        var config = await GetCachedConfigAsync();
        var validationKey = await GetValidationKeyAsync();

        return new TokenValidationParameters
        {
            ValidIssuer = config.Issuer,
            ValidateIssuer = true,
            ValidAudience = config.Audience,
            ValidateAudience = true,
            IssuerSigningKey = validationKey,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    }

    public async Task<RsaSecurityKey> GetSigningKeyAsync()
    {
        var config = await GetCachedConfigAsync();

        if (string.IsNullOrEmpty(config.PrivateKey))
        {
            throw new InvalidOperationException("JWT private key is not configured");
        }

        var rsa = RSA.Create();
        try
        {
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(config.PrivateKey), out _);
            return new RsaSecurityKey(rsa);
        }
        catch (Exception ex)
        {
            rsa.Dispose();
            _logger.LogError(ex, "Failed to import RSA private key");
            throw new InvalidOperationException("Invalid JWT private key format", ex);
        }
    }

    public async Task<RsaSecurityKey> GetValidationKeyAsync()
    {
        var config = await GetCachedConfigAsync();

        if (string.IsNullOrEmpty(config.PublicKey))
        {
            throw new InvalidOperationException("JWT public key is not configured");
        }

        var rsa = RSA.Create();
        try
        {
            rsa.ImportRSAPublicKey(Convert.FromBase64String(config.PublicKey), out _);
            return new RsaSecurityKey(rsa);
        }
        catch (Exception ex)
        {
            rsa.Dispose();
            _logger.LogError(ex, "Failed to import RSA public key");
            throw new InvalidOperationException("Invalid JWT public key format", ex);
        }
    }

    public async Task<string> GetIssuerAsync()
    {
        var config = await GetCachedConfigAsync();
        return config.Issuer;
    }

    public async Task<string> GetAudienceAsync()
    {
        var config = await GetCachedConfigAsync();
        return config.Audience;
    }

    public async Task<int> GetExpireMinutesAsync()
    {
        var config = await GetCachedConfigAsync();
        return config.ExpireMinutes;
    }

    public async Task<JwtConfig> GetJwtConfigAsync()
    {
        return await GetCachedConfigAsync();
    }

    public async Task RefreshCacheAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            _cachedConfig = null;
            _lastCacheUpdate = DateTime.MinValue;
            _logger.LogInformation("JWT configuration cache refreshed");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<JwtConfig> GetCachedConfigAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_cachedConfig != null && DateTime.UtcNow - _lastCacheUpdate < _cacheDuration)
            {
                return _cachedConfig;
            }

            _cachedConfig = await _configurationService.GetJwtConfigAsync();
            _lastCacheUpdate = DateTime.UtcNow;
            _logger.LogInformation("JWT configuration loaded from database");
            return _cachedConfig;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load JWT configuration from database");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}