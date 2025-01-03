using System.ComponentModel.DataAnnotations;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public class SmtpMailSenderOptions
{
    [Required]
    [MinLength(4)]
    public string Host { get; set; } = null!;

    public ushort Port { get; set; } = 25;

    public string? Username { get; set; }

    public string? Password { get; set; }
}