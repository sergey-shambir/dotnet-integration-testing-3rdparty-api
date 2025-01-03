using DailyRates.Modules.CurrencyRates.Application;
using DailyRates.Modules.CurrencyRates.Infrastructure.Cbr;
using Microsoft.Extensions.DependencyInjection;

namespace DailyRates.Modules.CurrencyRates.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddCurrencyRatesModule(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyRatesDataSource, CbrCurrencyRatesDataSource>();
    }
}