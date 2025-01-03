using System.Globalization;
using DailyRates.Modules.CurrencyRates.Application;

namespace DailyRates.Modules.CurrencyRates.Infrastructure.Cbr;

/// <summary>
///  Загружает курсы валют по данным Центрального Банка Российской Федерации (ЦБ РФ).
/// </summary>
/// l<see href="https://cbr.ru/development/SXML/"/>
public class CbrCurrencyRatesDataSource(HttpClient httpClient) : ICurrencyRatesDataSource
{
    public async Task<CurrencyRatesData> LoadCurrencyRates(DateOnly date)
    {
        string formattedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        HttpResponseMessage result = await httpClient.GetAsync(
            $"https://cbr.ru/scripts/XML_daily.asp?date_req={formattedDate}"
        );
        if (!result.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to fetch currency rate data from cbr.ru: {result.StatusCode}");
        }

        Stream responseStream = await result.Content.ReadAsStreamAsync();
        return CbrRuCurrencyRatesParser.Parse(responseStream);
    }
}