using DailyRates.Modules.Mailing.Application;

namespace CurrencyRates.Specs.TestDoubles.Modules.Mailing;

public class MockMailSender : IMailSender
{
    private readonly List<MailMessage> _sentMails = [];

    public IReadOnlyList<MailMessage> SentMails => _sentMails;

    public MailMessage FindMailByToName(string toName)
    {
        return SentMails.Single(message => message.ToName == toName);
    }

    public Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default)
    {
        _sentMails.Add(mail);
        return Task.CompletedTask;
    }
}