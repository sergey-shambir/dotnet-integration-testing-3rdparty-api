using DailyRates.Modules.Mailing.Application;
using MimeKit;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public static class MimeMessageFactory
{
    public static MimeMessage Create(MailMessage mail)
    {
        BodyBuilder bodyBuilder = new()
        {
            TextBody = mail.ContentPlainText,
        };

        MimeMessage message = new();
        message.From.Add(new MailboxAddress(mail.FromName, mail.FromEmail));
        message.To.Add(new MailboxAddress(mail.ToName, mail.ToEmail));
        message.Subject = mail.Subject;
        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}