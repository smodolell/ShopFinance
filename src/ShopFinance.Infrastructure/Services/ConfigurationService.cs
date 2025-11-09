using Ardalis.Result;
using ShopFinance.Application.Common.Interfaces;
using ShopFinance.Application.Common.Models.Configurations;
using ShopFinance.Domain.Common.Interfaces;
using ShopFinance.Domain.Entities;
using ShopFinance.Infrastructure.Constants;
using System.Security.Cryptography;

namespace ShopFinance.Infrastructure.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ConfigurationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<JwtConfig> GetJwtConfigAsync()
    {
        var issuer = await GetSettingValueAsync(JwtConstant.JwtIssuer);
        var audience = await GetSettingValueAsync(JwtConstant.JwtAudience);
        var privateKey = await GetSettingValueAsync(JwtConstant.JwtSigningRsaPrivateKey);
        var publicKey = await GetSettingValueAsync(JwtConstant.JwtSigningRsaPublicKey);
        var expireMinutes = await GetSettingValueAsync(JwtConstant.JwtExpireMinutes);

        return new JwtConfig
        {
            Issuer = issuer,
            Audience = audience,
            PrivateKey = privateKey,
            PublicKey = publicKey,
            ExpireMinutes = int.Parse(expireMinutes)
        };
    }

    private async Task<string> GetSettingValueAsync(string key)
    {
        var setting = await _unitOfWork.Settings.GetByKeyAsync(key);
        return setting?.Value ?? throw new InvalidOperationException($"Setting '{key}' not found");
    }

    public async Task InitializeDefaultSettingsAsync()
    {
        await InitializeJwtSettingsAsync();
        await InitializeEmailSettingsAsync();
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task InitializeJwtSettingsAsync()
    {
        var rsa = RSA.Create();

        var defaultSettings = new[]
        {
            new { Key = JwtConstant.JwtIssuer, Value = "ShopFinance", IsEncrypted = false },
            new { Key = JwtConstant.JwtAudience, Value = "ShopFinanceAudience", IsEncrypted = false },
            new { Key = JwtConstant.JwtSigningRsaPrivateKey, Value = Convert.ToBase64String(rsa.ExportRSAPrivateKey()), IsEncrypted = true },
            new { Key = JwtConstant.JwtSigningRsaPublicKey, Value = Convert.ToBase64String(rsa.ExportRSAPublicKey()), IsEncrypted = false },
            new { Key = JwtConstant.JwtExpireMinutes, Value = "60", IsEncrypted = false }
        };

        foreach (var defaultSetting in defaultSettings)
        {
            var exists = await _unitOfWork.Settings.KeyExistsAsync(defaultSetting.Key);
            if (!exists)
            {
                var setting = Setting.Create(
                    Guid.NewGuid(),
                    defaultSetting.Key,
                    defaultSetting.Value,
                    $"Default {defaultSetting.Key}",
                    defaultSetting.IsEncrypted);

                await _unitOfWork.Settings.AddAsync(setting);
            }
        }
    }

    private async Task InitializeEmailSettingsAsync()
    {
        var defaultEmailSettings = new[]
        {
            new { Key = EmailConstants.SmtpServer, Value = "smtp.gmail.com", IsEncrypted = false },
            new { Key = EmailConstants.SmtpPort, Value = "587", IsEncrypted = false },
            new { Key = EmailConstants.FromAddress, Value = "noreply@shopfinance.com", IsEncrypted = false },
            new { Key = EmailConstants.FromAddressTitle, Value = "ShopFinance System", IsEncrypted = false },
            new { Key = EmailConstants.SmtpUsername, Value = "your-email@gmail.com", IsEncrypted = false },
            new { Key = EmailConstants.SmtpPassword, Value = "your-app-password", IsEncrypted = true }
        };

        foreach (var defaultSetting in defaultEmailSettings)
        {
            var exists = await _unitOfWork.Settings.KeyExistsAsync(defaultSetting.Key);
            if (!exists)
            {
                var setting = Setting.Create(
                    Guid.NewGuid(),
                    defaultSetting.Key,
                    defaultSetting.Value,
                    $"Default {defaultSetting.Key}",
                    defaultSetting.IsEncrypted);

                await _unitOfWork.Settings.AddAsync(setting);
            }
        }
    }

    public async Task<EmailConfig> GetEmailConfigAsync()
    {
        var smtpServer = await GetSettingValueAsync(EmailConstants.SmtpServer);
        var smtpPort = await GetSettingValueAsync(EmailConstants.SmtpPort);
        var fromAddress = await GetSettingValueAsync(EmailConstants.FromAddress);
        var fromAddressTitle = await GetSettingValueAsync(EmailConstants.FromAddressTitle);
        var smtpUsername = await GetSettingValueAsync(EmailConstants.SmtpUsername);
        var smtpPassword = await GetSettingValueAsync(EmailConstants.SmtpPassword);

        return new EmailConfig
        {
            SmtpServer = smtpServer,
            SmtpPort = Convert.ToInt32(smtpPort),
            FromAddress = fromAddress,
            FromAddressTitle = fromAddressTitle,
            SmtpUsername = smtpUsername,
            SmtpPassword = smtpPassword
        };
    }

    public async Task<Result> SaveEmailConfigAsync(EmailConfig config)
    {
        try
        {
            await UpdateSettingAsync(EmailConstants.SmtpServer, config.SmtpServer);
            await UpdateSettingAsync(EmailConstants.SmtpPort, config.SmtpPort.ToString());
            await UpdateSettingAsync(EmailConstants.FromAddress, config.FromAddress);
            await UpdateSettingAsync(EmailConstants.FromAddressTitle, config.FromAddressTitle);
            await UpdateSettingAsync(EmailConstants.SmtpUsername, config.SmtpUsername);
            await UpdateSettingAsync(EmailConstants.SmtpPassword, config.SmtpPassword, true);

            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessWithMessage("Email configuration updated successfully");
        }
        catch (Exception ex)
        {
            return Result.Error($"Error saving email configuration: {ex.Message}");
        }
    }

    public async Task<Result> SaveJwtConfigAsync(JwtConfig config)
    {
        try
        {
            await UpdateSettingAsync(JwtConstant.JwtIssuer, config.Issuer);
            await UpdateSettingAsync(JwtConstant.JwtAudience, config.Audience);
            await UpdateSettingAsync(JwtConstant.JwtExpireMinutes, config.ExpireMinutes.ToString());

            // Solo actualizar las claves RSA si se proporcionan nuevas
            if (!string.IsNullOrEmpty(config.PrivateKey))
            {
                await UpdateSettingAsync(JwtConstant.JwtSigningRsaPrivateKey, config.PrivateKey, true);
            }

            if (!string.IsNullOrEmpty(config.PublicKey))
            {
                await UpdateSettingAsync(JwtConstant.JwtSigningRsaPublicKey, config.PublicKey);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.SuccessWithMessage("JWT configuration updated successfully");
        }
        catch (Exception ex)
        {
            return Result.Error($"Error saving JWT configuration: {ex.Message}");
        }
    }

    private async Task UpdateSettingAsync(string key, string value, bool isEncrypted = false)
    {
        var setting = await _unitOfWork.Settings.GetByKeyAsync(key);

        if (setting == null)
        {
            // Si no existe, crear nuevo setting
            setting = Setting.Create(
                Guid.NewGuid(),
                key,
                value,
                $"Configuration for {key}",
                isEncrypted);

            await _unitOfWork.Settings.AddAsync(setting);
        }
        else
        {
            // Si existe, actualizar valor
            setting.UpdateValue(value, isEncrypted);
           await _unitOfWork.Settings.UpdateAsync(setting);
        }
    }

    // Método adicional para obtener todas las configuraciones (útil para UI de administración)
    public async Task<Dictionary<string, string>> GetAllSettingsAsync()
    {
        var settings = await _unitOfWork.Settings.GetAllAsync();
        return settings.ToDictionary(s => s.Key, s => s.Value);
    }


}