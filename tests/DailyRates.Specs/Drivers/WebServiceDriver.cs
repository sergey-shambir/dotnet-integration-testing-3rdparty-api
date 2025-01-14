using System.Net.Http.Json;

namespace DailyRates.Specs.Drivers;

public class WebServiceDriver(HttpClient httpClient)
{
    public async Task Subscribe(string name, string email, List<string> currencyCodes)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "/api/currency-rates/subscribe",
            new
            {
                Name = name,
                Email = email,
                CurrencyCodes = currencyCodes,
            }
        );
        await EnsureSuccessStatusCode(response);
    }

    public async Task SendMails(DateOnly? currentDate)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "/api/currency-rates/send-mails",
            new
            {
                CurrentDate = currentDate,
            }
        );
        await EnsureSuccessStatusCode(response);
    }

    private async Task EnsureSuccessStatusCode(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"HTTP Status code {response.StatusCode}: {content}",
                null,
                response.StatusCode
            );
        }
    }
}