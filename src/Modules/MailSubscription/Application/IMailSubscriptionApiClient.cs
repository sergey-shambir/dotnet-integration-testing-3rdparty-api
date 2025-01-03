namespace DailyRates.Modules.MailSubscription.Application;

public interface IMailSubscriptionApiClient
{
    public Task AddUnconfirmedMailSubscription<TCustomData>(
        string subscriptionType,
        MailSubscriptionData<TCustomData> data
    ) where TCustomData : class;

    public Task<List<MailSubscriptionData<TCustomData>>> ListActiveMailSubscriptions<TCustomData>(
        string subscriptionType
    ) where TCustomData : class;
}