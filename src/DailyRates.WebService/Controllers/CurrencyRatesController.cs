using System.ComponentModel.DataAnnotations;
using DailyRates.Modules.MailSubscription.Application;
using Microsoft.AspNetCore.Mvc;

namespace DailyRates.WebService.Controllers;

[Route("api/currency-rates")]
[ApiController]
public class CurrencyRatesController(
    CurrencyRatesMailService currencyRatesMailService
) : ControllerBase
{
    [HttpPost("subscribe")]
    public async Task Subscribe(
        [FromBody] SubscribeRequest request
    )
    {
        await currencyRatesMailService.SubscribeToCurrencyRates(request.Name, request.Email, request.CurrencyCodes);
    }

    [HttpPost("send-mails")]
    public async Task SendMails(
        [FromBody] SendMailsRequest request
    )
    {
        DateOnly currentDate = request.CurrentDate ?? DateOnly.FromDateTime(DateTime.Now);
        await currencyRatesMailService.SendCurrencyRatesMails(currentDate);
    }

    public record SubscribeRequest
    {
        [Required]
        public string Name { get; init; } = null!;

        [Required]
        public string Email { get; init; } = null!;

        [Required]
        public List<string> CurrencyCodes { get; init; } = [];
    }

    public record SendMailsRequest
    {
        public DateOnly? CurrentDate { get; init; }
    }
}