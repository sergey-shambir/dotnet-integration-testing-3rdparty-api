using System.Runtime.Serialization;

namespace DailyRates.Modules.MailSubscription.Application;

[DataContract]
public class CurrencyCodesList(List<string> currencyCodes)
{
    [DataMember(Name = "currencyCodes")]
    public List<string> CurrencyCodes { get; set; } = currencyCodes;
}