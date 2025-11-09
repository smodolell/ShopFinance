using ShopFinance.Application.Common.Models.Configurations;

namespace ShopFinance.Application.Common.Interfaces;

public interface IConfigurationService
{
    Task<JwtConfig> GetJwtConfigAsync();
    Task<Result> SaveJwtConfigAsync(JwtConfig config);
    Task InitializeDefaultSettingsAsync();

    Task<EmailConfig> GetEmailConfigAsync();

    Task<Result> SaveEmailConfigAsync(EmailConfig config);

    Task<Dictionary<string, string>> GetAllSettingsAsync();



}
