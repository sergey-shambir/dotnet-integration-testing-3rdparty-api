using DailyRates.Modules.Mailing.Application;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DailyRates.Modules.Mailing.Infrastructure.MailKit;

public class SmtpMailSender(IOptionsSnapshot<SmtpMailSenderOptions> optionsSnapshot) : IMailSender, IAsyncDisposable
{
    private SmtpClient? _smtpClient;

    public async Task SendEmail(MailMessage mail, CancellationToken cancellationToken = default)
    {
        MimeMessage mimeMessage = MimeMessageFactory.Create(mail);

        _smtpClient ??= await CreateSmtpClient(cancellationToken);
        await _smtpClient.SendAsync(mimeMessage, cancellationToken);
    }

    private async Task<SmtpClient> CreateSmtpClient(CancellationToken cancellationToken)
    {
        SmtpMailSenderOptions options = optionsSnapshot.Value;

        SmtpClient smtpClient = new();
        await smtpClient.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls, cancellationToken);

        if (options.Username != null && options.Password != null)
        {
            await smtpClient.AuthenticateAsync(options.Username, options.Password, cancellationToken);
        }

        return smtpClient;
    }

    public async ValueTask DisposeAsync()
    {
        if (_smtpClient is { IsConnected: true })
        {
            await _smtpClient.DisconnectAsync(true);
        }

        _smtpClient?.Dispose();
        GC.SuppressFinalize(this);
    }
}