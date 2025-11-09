//using Microsoft.IdentityModel.Tokens;
//using ShopFinance.Application.Common.Interfaces;
//using ShopFinance.Domain.Entities;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace ShopFinance.Infrastructure.Services;

//public class JwtService : IJwtService
//{
//    private readonly IJwtConfigurationProvider _jwtConfigProvider;

//    public JwtService(IJwtConfigurationProvider jwtConfigProvider)
//    {
//        _jwtConfigProvider = jwtConfigProvider;
//    }
//    public async Task<string> GenerateToken(User user, IEnumerable<string> roles)
//    {
//        var signingKey = await _jwtConfigProvider.GetSigningKeyAsync();
//        var issuer = await _jwtConfigProvider.GetIssuerAsync();
//        var audience = await _jwtConfigProvider.GetAudienceAsync();

//        var tokenHandler = new JwtSecurityTokenHandler();
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Email, user.Email??""),
//                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
//                //new Claim(ClaimTypes.Role, user.Role)
//            }),
//            Expires = DateTime.UtcNow.AddHours(2),
//            Issuer = issuer,
//            Audience = audience,
//            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256)
//        };

//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }
//}
