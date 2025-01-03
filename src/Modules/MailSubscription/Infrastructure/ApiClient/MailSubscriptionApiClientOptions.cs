using System.ComponentModel.DataAnnotations;

namespace DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;

public class MailSubscriptionApiClientOptions
{
    [Required]
    public string ApiBaseUrl { get; set; } = string.Empty;
}