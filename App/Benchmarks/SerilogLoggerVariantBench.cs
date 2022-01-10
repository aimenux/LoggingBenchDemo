using BenchmarkDotNet.Attributes;
using Serilog;
using Serilog.Events;
using static App.Helpers.RandomHelper;
using ILogger = Serilog.ILogger;

namespace App.Benchmarks;

[Config(typeof(BenchConfig))]
[BenchmarkCategory(nameof(BenchCategory.SerilogLogger))]
public class SerilogLoggerVariantBench
{
    private string _message;
    private string _template;
    private ILogger _logger;

    public static int RandomNumber => Random.Shared.Next();

    [Params(50)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _message = RandomString(Length);
        _template = $"{_message} {{number}}";
        _logger = CreateSerilogLogger();
    }

    [Benchmark]
    public void NoParamsTraceLogging()
    {
        _logger.Verbose(_message);
    }

    [Benchmark]
    public void NoParamsTraceLoggingUsingIsEnabled()
    {
        if (_logger.IsEnabled(LogEventLevel.Verbose))
        {
            _logger.Verbose(_message);
        }
    }

    [Benchmark]
    public void WithParamsTraceLogging()
    {
        _logger.Verbose(_template, RandomNumber);
    }

    [Benchmark]
    public void WithParamsTraceLoggingUsingIsEnabled()
    {
        if (_logger.IsEnabled(LogEventLevel.Verbose))
        {
            _logger.Verbose(_template, RandomNumber);
        }
    }

    private static ILogger CreateSerilogLogger()
    {
        const LogEventLevel minLogEventLevel = LogEventLevel.Information;

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level >= minLogEventLevel).WriteTo.Console())
            .CreateLogger();

        return logger;
    }
}