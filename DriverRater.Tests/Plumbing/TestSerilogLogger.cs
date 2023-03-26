namespace DriverRater.Tests.Plumbing;

using Serilog;
using Serilog.Core;
using Xunit.Abstractions;

public static class TestSerilogLogger
{
    public static Logger CreateTestLogger(ITestOutputHelper output)
    {
        return new LoggerConfiguration()
            .MinimumLevel.Warning()
            .WriteTo.TestOutput(output)
            .WriteTo.Seq("http://localhost:5341")
            .Enrich.FromLogContext()
            .CreateLogger();
    }
}