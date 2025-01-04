using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace CurrencyRates.Specs.TestDoubles.Modules.MailSubscription;

public class FakeMailSubscriptionApiServer : DelegatingHandler
{
    private readonly Dictionary<string, List<MailSubscription>> _subscriptionsByTypeMap = [];

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (request.Method == HttpMethod.Post && request.RequestUri!.AbsolutePath == "/mail-subscription/")
        {
            return await AddUnconfirmedMailSubscription(request, cancellationToken);
        }

        if (request.Method == HttpMethod.Get && request.RequestUri!.AbsolutePath == "/mail-subscription/")
        {
            return ListMailSubscriptions(request);
        }

        throw new NotImplementedException(
            $"Fake does not support API method {request.Method} {request.RequestUri!.AbsolutePath}"
        );
    }

    private async Task<HttpResponseMessage> AddUnconfirmedMailSubscription(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        AddMailSubscriptionRequest data = await ReadFromJsonRequestBody<AddMailSubscriptionRequest>(
            request,
            cancellationToken
        );
        if (!_subscriptionsByTypeMap.TryGetValue(data.Type, out List<MailSubscription>? subscriptions))
        {
            subscriptions = [];
            _subscriptionsByTypeMap[data.Type] = subscriptions;
        }

        subscriptions.Add(new MailSubscription
        {
            Email = data.Email,
            Name = data.Name,
            CustomData = data.CustomData
        });

        return new HttpResponseMessage(HttpStatusCode.OK);
    }

    private HttpResponseMessage ListMailSubscriptions(HttpRequestMessage request)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri!.Query);
        string type = query["type"] ?? string.Empty;

        if (!_subscriptionsByTypeMap.TryGetValue(type, out List<MailSubscription>? subscriptions))
        {
            subscriptions = [];
        }

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(subscriptions),
        };
    }

    private async Task<T> ReadFromJsonRequestBody<T>(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        T? result = await request.Content!.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        return result ?? throw new NullReferenceException();
    }

    private class AddMailSubscriptionRequest : MailSubscription
    {
        public string Type { get; set; } = string.Empty;
    }

    private class MailSubscription
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public object? CustomData { get; set; }
    }
}