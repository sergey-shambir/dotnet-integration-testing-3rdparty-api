using System.ComponentModel.DataAnnotations;

namespace Mailing.Application;

public class MailingServiceOptions
{
    [Required]
    public string NoReplyName { get; set; } = null!;

    [Required]
    public string NoReplyEmail { get; set; } = null!;
}