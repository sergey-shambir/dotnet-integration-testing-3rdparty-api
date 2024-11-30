using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CurrencyRates.Domain;

namespace CurrencyRates.Infrastructure.Cbr;

public static class CbrRuCurrencyRatesParser
{
    public static CurrencyRatesData Parse(Stream stream)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encoding = Encoding.GetEncoding("windows-1251");
        StreamReader reader = new StreamReader(stream, encoding);

        XmlSerializer serializer = new(typeof(CbrCurrencyRates));
        if (serializer.Deserialize(reader) is not CbrCurrencyRates parsedRates)
        {
            throw new InvalidOperationException($"Failed to parse currency rate data");
        }

        CurrencyRatesData currencyRates = new CurrencyRatesData
        {
            CurrencyNames = { ["RUB"] = "Российский рубль" }
        };

        CultureInfo sourceCulture = CultureInfo.GetCultureInfo("ru-RU");
        foreach (CbrCurrencyData data in parsedRates.Currencies)
        {
            decimal exchangeRate = decimal.Parse(data.ExchangeRate, sourceCulture);
            currencyRates.CurrencyNames[data.Code] = data.Name;
            currencyRates.ExchangeRates[("RUB", data.Code)] = exchangeRate;
        }

        return currencyRates;
    }

    [XmlRoot("ValCurs")]
    public class CbrCurrencyRates
    {
        [XmlElement("Valute")]
        public List<CbrCurrencyData> Currencies { get; set; } = [];
    }

    public class CbrCurrencyData
    {
        [XmlElement("CharCode")]
        public string Code { get; set; } = null!;

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("VunitRate")]
        public string ExchangeRate { get; set; } = null!;
    }
}