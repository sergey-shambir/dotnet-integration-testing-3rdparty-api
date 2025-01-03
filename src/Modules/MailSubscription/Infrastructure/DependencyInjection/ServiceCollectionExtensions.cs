using DailyRates.Modules.MailSubscription.Application;
using DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyRates.Modules.MailSubscription.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddMailSubscriptionModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CurrencyRatesMailService>();
        services.AddScoped<IMailSubscriptionApiClient, MailSubscriptionApiClient>();
    }
}