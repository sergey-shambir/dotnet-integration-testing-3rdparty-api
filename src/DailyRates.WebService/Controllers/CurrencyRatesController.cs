using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DailyRates.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        [HttpPost("subscribe")]
        public void Subscribe(SubscribeRequest request)
        {
            throw new NotImplementedException();
        }
    }

    public record SubscribeRequest
    {
        [Required]
        public string Name { get; init; } = null!;

        [Required]
        public string Email { get; init; } = null!;
    }
}