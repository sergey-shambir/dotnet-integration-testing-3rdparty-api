using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Web;
using DailyRates.Modules.MailSubscription.Application;

namespace DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;

public class MailSubscriptionApiClient(HttpClient httpClient) : IMailSubscriptionApiClient
{
    public async Task AddUnconfirmedMailSubscription<TCustomData>(
        string subscriptionType,
        MailSubscriptionData<TCustomData> data
    ) where TCustomData : class
    {
        await httpClient.PostAsJsonAsync(
            "/mail-subscription/",
            new AddMailSubscriptionRequest<TCustomData>
            {
                Type = subscriptionType,
                Name = data.Name,
                Email = data.Email,
                CustomData = data.CustomData,
            }
        );
    }

    public async Task<List<MailSubscriptionData<TCustomData>>> ListActiveMailSubscriptions<TCustomData>(
        string subscriptionType
    ) where TCustomData : class
    {
        Uri uri = BuildUri("/mail-subscription/", new Dictionary<string, string>
        {
            { "type", subscriptionType },
        });

        List<MailSubscriptionData<TCustomData>>? results = await httpClient
            .GetFromJsonAsync<List<MailSubscriptionData<TCustomData>>>(uri);
        return results ?? [];
    }

    private static Uri BuildUri(string urlPath, Dictionary<string, string> queryParams)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        foreach ((string name, string value) in queryParams)
        {
            query.Add(name, value);
        }

        UriBuilder uriBuilder = new(urlPath)
        {
            Query = query.ToString(),
        };

        return uriBuilder.Uri;
    }

    private class AddMailSubscriptionRequest<TCustomData>
        : MailSubscription<TCustomData>
        where TCustomData : class
    {
        public string Type { get; set; } = string.Empty;
    }

    private class MailSubscription<TCustomData> where TCustomData : class
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TCustomData? CustomData { get; set; }
    }
}