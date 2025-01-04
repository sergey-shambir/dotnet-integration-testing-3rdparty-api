using DailyRates.Modules.Mailing.Application;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public class SmtpMailSender(
    IOptionsSnapshot<SmtpMailSenderOptions> optionsSnapshot,
    ISmtpClient smtpClient
) : IMailSender, IAsyncDisposable
{
    public async Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default)
    {
        MimeMessage mimeMessage = MimeMessageFactory.Create(mail);

        await ConnectOnce(cancellationToken);
        await smtpClient.SendAsync(mimeMessage, cancellationToken);
    }

    private async Task ConnectOnce(CancellationToken cancellationToken)
    {
        if (smtpClient.IsConnected)
        {
            return;
        }

        SmtpMailSenderOptions options = optionsSnapshot.Value;
        await smtpClient.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls, cancellationToken);
        if (options.Username != null && options.Password != null)
        {
            await smtpClient.AuthenticateAsync(options.Username, options.Password, cancellationToken);
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