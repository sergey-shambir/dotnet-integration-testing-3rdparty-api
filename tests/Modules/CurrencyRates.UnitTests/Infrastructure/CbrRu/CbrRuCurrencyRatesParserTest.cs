using DailyRates.Modules.CurrencyRates.Application;
using DailyRates.Modules.CurrencyRates.Infrastructure.Cbr;
using DailyRates.Tests.Modules.CurrencyRates.UnitTests.Data;

namespace DailyRates.Tests.Modules.CurrencyRates.UnitTests.Infrastructure.CbrRu;

public class CbrRuCurrencyRatesParserTest
{
    [Fact]
    public void Can_parse_currency_rates_from_cbr_ru()
    {
        using Stream inputStream = TestDataLoader.OpenFileForReading(Path.Combine("2024-07-10", "XML_daily.xml"));

        CurrencyRatesData data = CbrRuCurrencyRatesParser.Parse(inputStream);

        Assert.Equivalent(new Dictionary<string, string>
        {
            { "AUD", "Австралийский доллар" },
            { "AZN", "Азербайджанский манат" },
            { "GBP", "Фунт стерлингов Соединенного королевства" },
            { "AMD", "Армянских драмов" },
            { "BGN", "Болгарский лев" },
            { "BRL", "Бразильский реал" }
        }, data.CurrencyNames);

        Assert.Equal(59.3493m, data.ExchangeRates[("RUB", "AUD")]);
        Assert.Equal(51.7665m, data.ExchangeRates[("RUB", "AZN")]);
        Assert.Equal(112.9344m, data.ExchangeRates[("RUB", "GBP")]);
        Assert.Equal(0.226806m, data.ExchangeRates[("RUB", "AMD")]);
        Assert.Equal(48.7525m, data.ExchangeRates[("RUB", "BGN")]);
        Assert.Equal(16.0833m, data.ExchangeRates[("RUB", "BRL")]);
    }
}