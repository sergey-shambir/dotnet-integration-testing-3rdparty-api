using DailyRates.Modules.CurrencyRates.Infrastructure.DependencyInjection;
using DailyRates.Modules.Mailing.Infrastructure.DependencyInjection;
using DailyRates.Modules.MailSubscription.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddCurrencyRatesModule();
builder.Services.AddMailingModule(builder.Configuration);
builder.Services.AddMailSubscriptionModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program;