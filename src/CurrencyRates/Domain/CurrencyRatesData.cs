namespace CurrencyRates.Domain;

public class CurrencyRatesData
{
    /// <summary>
    ///  Отображает валютную пару на курс конвертации из первой валюты во вторую.
    ///  Например: (RUB, AED) => 23.967 означает, что 1 RUB = 23.967 AED
    /// </summary>
    public Dictionary<(string, string), decimal> ExchangeRates { get; init; } = [];

    /// <summary>
    ///  Отображает код валюты на название.
    /// </summary>
    public Dictionary<string, string> CurrencyNames { get; init; } = [];
}