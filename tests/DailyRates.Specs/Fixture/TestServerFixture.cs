namespace DailyRates.Specs.Fixture;

public class TestServerFixture : IDisposable
{
    private readonly CustomWebApplicationFactory _factory = new();

    public HttpClient HttpClient { get; }

    public IServiceProvider ServiceProvider { get; }

    public TestServerFixture()
    {
        HttpClient = _factory.CreateClient();
        ServiceProvider = _factory.Services;
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        _factory.Dispose();
    }
}