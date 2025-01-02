namespace Mailing.Application;

public interface IMailSender
{
    public Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default);
}