using Mailing.Application;
using MimeKit;

namespace Mailing.Infrastructure.MailKit;

public static class MimeMessageFactory
{
    public static MimeMessage Create(MailMessage mail)
    {
        MailboxAddress fromAddress = new(mail.FromName, mail.FromEmail);
        MailboxAddress toAddress = new(mail.ToName, mail.ToEmail);

        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = mail.ContentHtml,
        };

        return new MimeMessage(fromAddress, toAddress, mail.Subject, bodyBuilder.ToMessageBody());
    }
}