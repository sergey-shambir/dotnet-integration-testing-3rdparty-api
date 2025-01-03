using System.Collections.Specialized;
using System.Net.Http.Json;
using System.Web;
using DailyRates.Modules.MailSubscription.Application;
using Microsoft.Extensions.Options;

namespace DailyRates.Modules.MailSubscription.Infrastructure.ApiClient;

public class MailSubscriptionApiClient(
    HttpClient httpClient,
    IOptionsSnapshot<MailSubscriptionApiClientOptions> optionsSnapshot
) : IMailSubscriptionApiClient
{
    public async Task AddUnconfirmedMailSubscription<TCustomData>(
        string subscriptionType,
        MailSubscriptionData<TCustomData> data
    ) where TCustomData : class
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            BuildApiUrl("/mail-subscription/"),
            new AddMailSubscriptionRequest<TCustomData>
            {
                Type = subscriptionType,
                Name = data.Name,
                Email = data.Email,
                CustomData = data.CustomData,
            }
        );
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<MailSubscriptionData<TCustomData>>> ListActiveMailSubscriptions<TCustomData>(
        string subscriptionType
    ) where TCustomData : class
    {
        Uri uri = BuildApiUrl(
            "/mail-subscription/",
            new Dictionary<string, string>
            {
                { "type", subscriptionType },
            }
        );

        List<MailSubscriptionData<TCustomData>>? results = await httpClient
            .GetFromJsonAsync<List<MailSubscriptionData<TCustomData>>>(uri);
        return results ?? [];
    }

    private Uri BuildApiUrl(string urlPath, Dictionary<string, string>? queryParams = null)
    {
        string apiBaseUrl = optionsSnapshot.Value.ApiBaseUrl;

        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        if (queryParams is not null)
        {
            foreach ((string name, string value) in queryParams)
            {
                query.Add(name, value);
            }
        }

        UriBuilder uriBuilder = new(apiBaseUrl)
        {
            Path = urlPath,
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