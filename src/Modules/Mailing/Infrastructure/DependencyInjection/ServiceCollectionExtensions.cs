using DailyRates.Modules.Mailing.Application;
using DailyRates.Modules.Mailing.Infrastructure.MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DailyRates.Modules.Mailing.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string MailingServiceSection = "Mailing:Service";
    private const string MailingSmtpSection = "Mailing:Smtp";

    public static void AddMailingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<MailingServiceOptions>()
            .Bind(configuration.GetRequiredSection(MailingServiceSection))
            .ValidateDataAnnotations();

        services.AddOptionsWithValidateOnStart<SmtpMailSenderOptions>()
            .Bind(configuration.GetRequiredSection(MailingSmtpSection))
            .ValidateDataAnnotations();

        services.AddTransient<ISmtpClient, SmtpClient>();
        services.AddScoped<IMailSender, SmtpMailSender>();
        services.AddScoped<MailingService>();
    }
}