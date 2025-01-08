using DailyRates.Modules.Mailing.Application;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public class SmtpMailSender(
    IOptionsSnapshot<SmtpMailSenderOptions> optionsSnapshot,
    ISmtpClient smtpClient,
    ILogger<SmtpMailSender> logger
) : IMailSender, IAsyncDisposable
{
    public async Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default)
    {
        MimeMessage mimeMessage = MimeMessageFactory.Create(mail);

        await ConnectOnce(cancellationToken);

        await smtpClient.SendAsync(mimeMessage, cancellationToken);

        string toEmailsList = string.Join(", ", mimeMessage.To.Select(x => x.ToString()));
        logger.LogInformation($"Sent email message '{mail.Subject}' to {toEmailsList}");
    }

    private async Task ConnectOnce(CancellationToken cancellationToken)
    {
        if (smtpClient.IsConnected)
        {
            return;
        }

        SmtpMailSenderOptions options = optionsSnapshot.Value;
        try
        {
            await smtpClient.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls, cancellationToken);
            logger.LogInformation($"Connected to smtp server '{options.Host}:{options.Port}'");
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Failed to connect mail server '{options.Host}:{options.Port}': {e.Message}", e
            );
        }

        if (options.Username != null && options.Password != null)
        {
            await smtpClient.AuthenticateAsync(options.Username, options.Password, cancellationToken);
            logger.LogInformation($"Authenticated on smtp server with username {options.Username}");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (smtpClient.IsConnected)
        {
            await smtpClient.DisconnectAsync(true);
        }

        smtpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}