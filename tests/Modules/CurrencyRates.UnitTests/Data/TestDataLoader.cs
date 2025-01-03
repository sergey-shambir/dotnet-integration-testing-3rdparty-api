using System.Runtime.CompilerServices;

namespace DailyRates.Tests.Modules.CurrencyRates.UnitTests.Data;

public static class TestDataLoader
{
    public static Stream OpenFileForReading(string relativePath)
    {
        string filePath = Path.Combine(GetDataDirectoryPath(), relativePath);
        return File.OpenRead(filePath);
    }

    private static string GetDataDirectoryPath([CallerFilePath] string path = "")
    {
        return Path.GetDirectoryName(path)
               ?? throw new ArgumentException($"Cannot get directory path from {path}");
    }
}