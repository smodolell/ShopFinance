using Microsoft.Extensions.Diagnostics.HealthChecks;
using ShopFinance.Application.Common.Interfaces;

namespace ShopFinance.Infrastructure.HealthChecks;

public class JwtConfigurationHealthCheck : IHealthCheck
{
    private readonly IJwtConfigurationProvider _jwtConfigProvider;

    public JwtConfigurationHealthCheck(IJwtConfigurationProvider jwtConfigProvider)
    {
        _jwtConfigProvider = jwtConfigProvider;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var validationParams = await _jwtConfigProvider.GetTokenValidationParametersAsync();

            if (validationParams.IssuerSigningKey == null)
                return HealthCheckResult.Unhealthy("JWT signing key is not configured");

            return HealthCheckResult.Healthy("JWT configuration is valid");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("JWT configuration check failed", ex);
        }
    }
}