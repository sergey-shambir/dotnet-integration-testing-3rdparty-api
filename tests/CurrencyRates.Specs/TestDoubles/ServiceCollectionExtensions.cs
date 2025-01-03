using CurrencyRates.Specs.TestDoubles.Modules.CurrencyRates;
using CurrencyRates.Specs.TestDoubles.Modules.MailSubscription;
using DailyRates.Modules.CurrencyRates.Application;
using DailyRates.Modules.CurrencyRates.Infrastructure.Cbr;
using DailyRates.Modules.MailSubscription.Application;
using DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRates.Specs.TestDoubles;

public static class ServiceCollectionExtensions
{
    private const string MailSubscriptionApiClient = "MailSubscriptionService";

    public static void AddTestDoubles(this IServiceCollection services)
    {
        services.AddCurrencyRatesModuleTestDoubles();
        services.AddMailSubscriptionModuleTestDoubles();
    }

    private static void AddCurrencyRatesModuleTestDoubles(this IServiceCollection services)
    {
        services.AddSingleton<StubCbrCurrencyRatesServer>();
        services.AddHttpClient<ICurrencyRatesDataSource, CbrCurrencyRatesDataSource>(nameof(CbrCurrencyRatesDataSource))
            .AddHttpMessageHandler<StubCbrCurrencyRatesServer>();
    }

    private static void AddMailSubscriptionModuleTestDoubles(this IServiceCollection services)
    {
        services.AddSingleton<FakeMailSubscriptionApiServer>();
        services.AddHttpClient<IMailSubscriptionApiClient, MailSubscriptionApiClient>(nameof(MailSubscriptionApiClient))
            .AddHttpMessageHandler<FakeMailSubscriptionApiServer>();
    }
}