namespace DailyRates.Modules.Mailing.Application;

public interface IMailSender
{
    public Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default);
}