//using Microsoft.Extensions.Logging;
//using Microsoft.IdentityModel.Tokens;
//using ShopFinance.Application.Common.Interfaces;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace ShopFinance.Infrastructure.Services;

//public class JwtHelper : IJwtHelper
//{
//    private readonly IJwtConfigurationProvider _configProvider;
//    private readonly ILogger<JwtHelper> _logger;

//    public JwtHelper(
//        IJwtConfigurationProvider configProvider,
//        ILogger<JwtHelper> logger)
//    {
//        _configProvider = configProvider;
//        _logger = logger;
//    }

//    public async Task<string> GenerateJwtToken(List<Claim> claims)
//    {
//        var signingKey = await _configProvider.GetSigningKeyAsync();
//        var issuer = await _configProvider.GetIssuerAsync();
//        var audience = await _configProvider.GetAudienceAsync();
//        var expireMinutes = await _configProvider.GetExpireMinutesAsync();

//        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(claims),
//            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
//            Issuer = issuer,
//            Audience = audience,
//            SigningCredentials = credentials
//        };

//        var tokenHandler = new JwtSecurityTokenHandler();
//        var token = tokenHandler.CreateToken(tokenDescriptor);

//        _logger.LogDebug("JWT token generated for user: {User}",
//            claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);

//        return tokenHandler.WriteToken(token);
//    }

//    public async Task<ClaimsPrincipal?> ValidToken(string token)
//    {
//        try
//        {
//            if (string.IsNullOrWhiteSpace(token))
//                return null;

//            var validationParameters = await _configProvider.GetTokenValidationParametersAsync();
//            var principal = new JwtSecurityTokenHandler()
//                .ValidateToken(token, validationParameters, out var validatedToken);

//            _logger.LogDebug("JWT token validated successfully");
//            return principal;
//        }
//        catch (SecurityTokenExpiredException ex)
//        {
//            _logger.LogWarning("JWT token expired: {Token}", token);
//            return null;
//        }
//        catch (SecurityTokenInvalidSignatureException ex)
//        {
//            _logger.LogWarning("JWT token has invalid signature: {Token}", token);
//            return null;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogWarning(ex, "JWT token validation failed");
//            return null;
//        }
//    }


//    public async Task<TokenValidationParameters> GetValidationParameters()
//    {
//        return await _configProvider.GetTokenValidationParametersAsync();
//    }
//}