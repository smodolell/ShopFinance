namespace ShopFinance.Application.Common.Models.Configurations;
public class JwtConfig
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public int ExpireMinutes { get; set; }
}
