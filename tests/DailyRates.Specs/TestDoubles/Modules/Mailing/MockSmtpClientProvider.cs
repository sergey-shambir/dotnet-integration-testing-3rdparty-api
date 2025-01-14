using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Moq;

namespace DailyRates.Specs.TestDoubles.Modules.Mailing;

public class MockSmtpClientProvider
{
    private readonly List<MimeMessage> _sentMails;
    private readonly Mock<ISmtpClient> _smtpClientMock;
    private bool _isConnected;

    public ISmtpClient SmtpClient => _smtpClientMock.Object;

    public MockSmtpClientProvider()
    {
        _sentMails = [];
        _smtpClientMock = new Mock<ISmtpClient>(MockBehavior.Strict);

        _smtpClientMock.Setup(
            x => x.ConnectAsync(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<SecureSocketOptions>(),
                It.IsAny<CancellationToken>()
            )
        ).Callback<string, int, SecureSocketOptions, CancellationToken>(
            (_, _, _, _) => _isConnected = true
        ).Returns(Task.CompletedTask);
        _smtpClientMock.Setup(x => x.IsConnected).Returns(() => _isConnected);

        _smtpClientMock.Setup(
            x => x.AuthenticateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            )
        ).Returns(Task.CompletedTask);
        _smtpClientMock.Setup(
            x => x.SendAsync(
                It.IsAny<MimeMessage>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<ITransferProgress>()
            )
        ).Callback<MimeMessage, CancellationToken, ITransferProgress>(
            (x, _, _) => _sentMails.Add(x)
        ).Returns(Task.FromResult(string.Empty));

        _smtpClientMock.Setup(x => x.Dispose());
    }

    public MimeMessage FindMailByToName(string toName)
    {
        return _sentMails.Single(message => message.To.Any(x => x.Name == toName));
    }
}