namespace Mailing.Application;

public record MailMessage(
    string FromName,
    string FromEmail,
    string ToName,
    string ToEmail,
    string Subject,
    string ContentHtml
);