using DailyRates.Specs.Drivers;
using DailyRates.Specs.Fixture;
using DailyRates.Specs.TestDoubles.Modules.Mailing;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Reqnroll;
using Reqnroll.Assist.Attributes;
using Xunit;

namespace DailyRates.Specs.Steps;

[Binding]
public class SubscribeStepDefinitions(TestServerFixture fixture)
{
    private readonly WebServiceDriver _driver = new(fixture.HttpClient);

    private MockSmtpClientProvider MockSmtpClientProvider =>
        fixture.ServiceProvider.GetRequiredService<MockSmtpClientProvider>();

    [Given(@"пользователи подписались на обновления курсов валют:")]
    public async Task ПустьПользователиПодписалисьНаОбновленияКурсовВалют(Table table)
    {
        List<SubscribeRequest> requests = table.CreateSet<SubscribeRequest>().ToList();
        foreach (SubscribeRequest request in requests)
        {
            await _driver.Subscribe(request.Name, request.Email, request.GetCurrencyCodesList());
        }
    }

    [When(@"загружаем курсы валют за ""(.*)"" и рассылаем письма")]
    public async Task КогдаЗагружаемКурсыВалютЗаИРассылаемПисьма(DateOnly date)
    {
        await _driver.SendMails(date);
    }

    [Then(@"""(.*)"" получит письмо ""(.*)"" с текстом:")]
    public void ТогдаПолучитПисьмоСТекстом(string name, string mailSubject, string mailContentPlainText)
    {
        MimeMessage mailMessage = MockSmtpClientProvider.FindMailByToName(name);
        Assert.Equal(mailSubject, mailMessage.Subject);
        Assert.Equal(mailContentPlainText.Trim(), mailMessage.TextBody.Trim());
    }

    private class SubscribeRequest
    {
        [TableAliases("Имя")]
        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        [TableAliases("Коды валют")]
        public string CurrencyCodes { get; init; } = string.Empty;

        public List<string> GetCurrencyCodesList()
        {
            return CurrencyCodes.Split(',').Select(x => x.Trim()).ToList();
        }
    }
}