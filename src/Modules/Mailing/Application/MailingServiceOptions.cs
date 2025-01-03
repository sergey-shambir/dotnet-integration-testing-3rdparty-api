using System.ComponentModel.DataAnnotations;

namespace DailyRates.Modules.Mailing.Application;

public class MailingServiceOptions
{
    [Required]
    public string NoReplyName { get; set; } = null!;

    [Required]
    public string NoReplyEmail { get; set; } = null!;
}