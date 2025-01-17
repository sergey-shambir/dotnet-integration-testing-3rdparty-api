using DailyRates.Modules.CurrencyRates.Application;
using DailyRates.Modules.CurrencyRates.Infrastructure.Cbr;
using DailyRates.Modules.MailSubscription.Application;
using DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;
using DailyRates.Specs.TestDoubles.Modules.CurrencyRates;
using DailyRates.Specs.TestDoubles.Modules.Mailing;
using DailyRates.Specs.TestDoubles.Modules.MailSubscription;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;

namespace DailyRates.Specs.TestDoubles;

public static class ServiceCollectionExtensions
{
    public static void AddTestDoubles(this IServiceCollection services)
    {
        services.AddCurrencyRatesModuleTestDoubles();
        services.AddMailingTestDoubles();
        services.AddMailSubscriptionModuleTestDoubles();
    }

    private static void AddCurrencyRatesModuleTestDoubles(this IServiceCollection services)
    {
        services.AddSingleton<StubCbrCurrencyRatesServer>();
        services.AddHttpClient<ICurrencyRatesDataSource, CbrCurrencyRatesDataSource>(nameof(CbrCurrencyRatesDataSource))
            .AddHttpMessageHandler<StubCbrCurrencyRatesServer>();
    }

    private static void AddMailingTestDoubles(this IServiceCollection services)
    {
        services.AddSingleton<MockSmtpClientProvider>();
        services.AddSingleton<ISmtpClient>(
            provider => provider.GetRequiredService<MockSmtpClientProvider>().SmtpClient
        );
    }

    private static void AddMailSubscriptionModuleTestDoubles(this IServiceCollection services)
    {
        services.AddSingleton<FakeMailSubscriptionApiServer>();
        services.AddHttpClient<IMailSubscriptionApiClient, MailSubscriptionApiClient>(nameof(MailSubscriptionApiClient))
            .AddHttpMessageHandler<FakeMailSubscriptionApiServer>();
    }
}