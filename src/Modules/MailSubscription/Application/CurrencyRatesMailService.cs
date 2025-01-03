using DailyRates.Modules.CurrencyRates.Application;
using DailyRates.Modules.Mailing.Application;

namespace DailyRates.Modules.MailSubscription.Application;

public class CurrencyRatesMailService(
    IMailSubscriptionApiClient apiClient,
    MailingService mailingService,
    ICurrencyRatesDataSource currencyRatesDataSource
)
{
    private const string CurrencyRatesMailType = "CurrencyRatesMail";
    private const string RuCurrency = "RUB";

    /// <summary>
    ///  Добавляет подписку на рассылки о курсах валют.
    /// </summary>
    public async Task SubscribeToCurrencyRates(string name, string email, List<string> currencyCodes)
    {
        await apiClient.AddUnconfirmedMailSubscription(
            CurrencyRatesMailType,
            new MailSubscriptionData<CurrencyCodesList>(
                name: name,
                email: email,
                customData: new CurrencyCodesList(
                    currencyCodes: currencyCodes
                )
            )
        );
    }

    /// <summary>
    ///  Отправляет письма с курсами валют.
    /// </summary>
    public async Task SendCurrencyRatesMails(DateOnly currentDate)
    {
        CurrencyRatesData currencyRates = await currencyRatesDataSource.LoadCurrencyRates(currentDate);

        List<MailSubscriptionData<CurrencyCodesList>> mailSubscriptions = await apiClient
            .ListActiveMailSubscriptions<CurrencyCodesList>(CurrencyRatesMailType);
        foreach (MailSubscriptionData<CurrencyCodesList> data in mailSubscriptions)
        {
            await SendCurrencyRatesMail(currentDate, currencyRates, data);
        }
    }

    private async Task SendCurrencyRatesMail(
        DateOnly currentDate,
        CurrencyRatesData currencyRates,
        MailSubscriptionData<CurrencyCodesList> data
    )
    {
        CurrencyCodesList customData = (CurrencyCodesList)data.CustomData!;
        Dictionary<string, decimal> currencyNameToExchangeRate = customData.CurrencyCodes.ToDictionary(
            currencyCode => currencyRates.CurrencyNames[currencyCode],
            currencyCode => currencyRates.ExchangeRates[(RuCurrency, currencyCode)]
        );

        RuCurrencyRatesMailTemplate mailTemplate = new(
            currentDate: currentDate,
            name: data.Name,
            currencyRates: currencyNameToExchangeRate
        );

        await mailingService.SendNotificationMail(
            data.Name,
            data.Email,
            mailTemplate.GetSubject(),
            mailTemplate.GetContentsPlainText()
        );
    }
}