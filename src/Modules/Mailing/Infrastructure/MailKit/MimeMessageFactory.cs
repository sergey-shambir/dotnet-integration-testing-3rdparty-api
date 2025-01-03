using DailyRates.Modules.Mailing.Application;
using MimeKit;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public static class MimeMessageFactory
{
    public static MimeMessage Create(MailMessage mail)
    {
        MailboxAddress fromAddress = new(mail.FromName, mail.FromEmail);
        MailboxAddress toAddress = new(mail.ToName, mail.ToEmail);

        BodyBuilder bodyBuilder = new()
        {
            TextBody = mail.ContentPlainText,
        };

        return new MimeMessage(fromAddress, toAddress, mail.Subject, bodyBuilder.ToMessageBody());
    }
}