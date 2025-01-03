using DailyRates.Modules.MailSubscription.Application;
using DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyRates.Modules.MailSubscription.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string MailSubscriptionSection = "MailSubscription";

    public static void AddMailSubscriptionModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<MailSubscriptionApiClientOptions>()
            .Bind(configuration.GetRequiredSection(MailSubscriptionSection))
            .ValidateDataAnnotations();

        services.AddScoped<CurrencyRatesMailService>();
        services.AddHttpClient<IMailSubscriptionApiClient, MailSubscriptionApiClient>();
    }
}