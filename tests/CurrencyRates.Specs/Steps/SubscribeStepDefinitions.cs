using System.Net.Http.Json;
using CurrencyRates.Specs.Drivers;
using CurrencyRates.Specs.Fixture;
using Reqnroll;
using Reqnroll.Assist.Attributes;

namespace CurrencyRates.Specs.Steps;

[Binding]
public class SubscribeStepDefinitions(TestServerFixture fixture)
{
    private readonly WebServiceDriver _driver = new(fixture.HttpClient);

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