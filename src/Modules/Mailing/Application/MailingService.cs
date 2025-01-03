using Microsoft.Extensions.Options;

namespace DailyRates.Modules.Mailing.Application;

public class MailingService(
    IOptionsSnapshot<MailingServiceOptions> options,
    IMailSender mailSender
)
{
    public Task SendNotificationMail(
        string toName,
        string toEmail,
        string subject,
        string contentPlainText
    )
    {
        MailMessage mail = new(
            FromName: options.Value.NoReplyName,
            FromEmail: options.Value.NoReplyEmail,
            ToName: toName,
            ToEmail: toEmail,
            Subject: subject,
            ContentPlainText: contentPlainText
        );

        return mailSender.SendEmail(mail);
    }
}