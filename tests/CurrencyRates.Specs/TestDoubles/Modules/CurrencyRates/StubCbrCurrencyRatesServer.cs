using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Web;
using CurrencyRates.Specs.Data;

namespace CurrencyRates.Specs.TestDoubles.Modules.CurrencyRates;

public class StubCbrCurrencyRatesServer : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (request.Method == HttpMethod.Get && request.RequestUri!.AbsolutePath == "/scripts/XML_daily.asp")
        {
            return Task.FromResult(ReadXmlDailyFile(request));
        }

        throw new NotImplementedException(
            $"Fake does not support API method {request.Method} {request.RequestUri!.AbsolutePath}"
        );
    }

    private static HttpResponseMessage ReadXmlDailyFile(HttpRequestMessage request)
    {
        NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri!.Query);
        DateOnly date = ParseDateFromQuery(query, "date_req", "dd/MM/yyyy");
        string path = Path.Combine(date.ToString("yyyy-MM-dd"), "XML_daily.xml");
        Stream content = TestDataLoader.OpenFileForReading(path);

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(content)
            {
                Headers =
                {
                    { "Content-Type", "application/xml; charset=windows-125" },
                }
            },
        };
    }

    private static DateOnly ParseDateFromQuery(NameValueCollection query, string parameterName, string format)
    {
        string dateString = query[parameterName] ?? string.Empty;
        DateTime dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
        return DateOnly.FromDateTime(dateTime);
    }
}