using CurrencyRates.Domain;
using CurrencyRates.Infrastructure.Cbr;

namespace CurrencyRates.UnitTests.Infrastructure.CbrRu;

public class CbrRuCurrencyRatesParserTest
{
    [Fact]
    public void Can_parse_currency_rates_from_cbr_ru()
    {
        string inputPath = Path.Combine(GetProjectDirectory(), "Data", "2024-07-10", "XML_daily.xml");
        Stream inputStream = File.OpenRead(inputPath);

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

        Assert.Equivalent(new Dictionary<(string, string), decimal>
        {
            { ("RUB", "AUD"), 59.3493m },
            { ("RUB", "AZN"), 51.7665m },
            { ("RUB", "GBP"), 112.9344m },
            { ("RUB", "AMD"), 22.6806m },
            { ("RUB", "BGN"), 48.7525m },
            { ("RUB", "BRL"), 16.0833m },
        }, data.ExchangeRates);
    }

    private static string GetProjectDirectory()
    {
        return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))!;
    }
}