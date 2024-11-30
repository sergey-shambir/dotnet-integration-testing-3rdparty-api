namespace CurrencyRates.Domain;

public interface ICurrencyRatesDataSource
{
    public Task<CurrencyRatesData> LoadCurrencyRates(DateOnly date);
}