namespace ShopFinance.Application.Common.Models.Configurations;

public class EmailConfig
{
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; }
    public string FromAddress { get; set; } = "";
    public string FromAddressTitle { get; set; } = "";
    public string SmtpUsername { get; set; } = "";
    public string SmtpPassword { get; set; } = "";
}
