namespace DailyRates.Modules.MailSubscription.Application;

public class MailSubscriptionData<TCustomData>(string name, string email, TCustomData? customData = null)
    where TCustomData : class
{
    public string Name { get; init; } = name;

    public string Email { get; init; } = email;

    public TCustomData? CustomData { get; init; } = customData;
}