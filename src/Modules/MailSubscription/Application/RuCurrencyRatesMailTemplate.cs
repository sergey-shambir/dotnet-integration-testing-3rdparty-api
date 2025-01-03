using System.Text;

namespace DailyRates.Modules.MailSubscription.Application;

public class RuCurrencyRatesMailTemplate(
    DateOnly currentDate,
    string name,
    Dictionary<string, decimal> currencyRates
)
{
    public string GetSubject()
    {
        return $"Курсы валют на {currentDate.ToString("yyyy-MM-dd")}";
    }

    public string GetContentsPlainText()
    {
        StringBuilder text = new();
        text.AppendLine($"Доброе утро, {name}!");
        text.AppendLine("Курсы валют на сегодня:");

        foreach ((string currencyName, decimal exchangeRate) in currencyRates)
        {
            text.AppendLine($"- 1 {currencyName} = {exchangeRate:f4} Рублей");
        }

        return text.ToString();
    }
}