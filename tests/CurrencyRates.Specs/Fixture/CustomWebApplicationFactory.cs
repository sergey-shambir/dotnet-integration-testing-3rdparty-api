using CurrencyRates.Specs.TestDoubles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace CurrencyRates.Specs.Fixture;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddTestDoubles();
            services.AddLogging(loggingBuilder => loggingBuilder
                .AddConsole()
                .AddFilter(level => level >= LogLevel.Trace)
            );
        });
    }
    
    
}