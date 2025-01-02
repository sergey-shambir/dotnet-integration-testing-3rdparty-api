using Microsoft.Extensions.Options;

namespace Mailing.Application;

public class MailingService(
    IOptionsSnapshot<MailingServiceOptions> options,
    IMailSender mailSender
)
{
    public Task SendNotificationMail(
        string toName,
        string toEmail,
        string subject,
        string contentHtml
    )
    {
        MailMessage mail = new(
            FromName: options.Value.NoReplyName,
            FromEmail: options.Value.NoReplyEmail,
            ToName: toName,
            ToEmail: toEmail,
            Subject: subject,
            ContentHtml: contentHtml
        );

        return mailSender.SendEmail(mail);
    }
}