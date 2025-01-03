namespace DailyRates.Modules.CurrencyRates.Application;

public interface ICurrencyRatesDataSource
{
    public Task<CurrencyRatesData> LoadCurrencyRates(DateOnly date);
}