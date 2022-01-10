using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using static App.Helpers.RandomHelper;

namespace App.Benchmarks;

[Config(typeof(BenchConfig))]
[BenchmarkCategory(nameof(BenchCategory.DefaultLogger))]
public class DefaultLoggerVariantBench
{
    private string _message;
    private string _template;
    private IDefaultLoggerAdapter _logger;
    public static int RandomNumber => Random.Shared.Next();

    [Params(50)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _message = RandomString(Length);
        _template = $"{_message} {{number}}";
        _logger = CreateDefaultLogger();
    }

    [Benchmark]
    public void NoParamsTraceLogging()
    {
        _logger.LogTrace(_message);
    }

    [Benchmark]
    public void WithParamsTraceLogging()
    {
        _logger.LogTrace(_template, RandomNumber);
    }

    private static IDefaultLoggerAdapter CreateDefaultLogger()
    {
        const LogLevel minLogLevel = LogLevel.Information;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minLogLevel);
            builder.AddFilter(level => level >= minLogLevel);
            builder.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss:fff] ");
        });

        var logger = new Logger<DefaultLoggerVariantBench>(loggerFactory);
        return new DefaultLoggerAdapter<DefaultLoggerVariantBench>(logger);
    }
}

public interface IDefaultLoggerAdapter
{
    void LogTrace(string message);

    void LogTrace<T0>(string template, T0 args0);
}

public interface IDefaultLoggerAdapter<T> : IDefaultLoggerAdapter
{
}

public class DefaultLoggerAdapter<T> : IDefaultLoggerAdapter<T>
{
    private readonly ILogger<T> _logger;

    public DefaultLoggerAdapter(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogTrace(string message)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(message);
        }
    }

    public void LogTrace<T0>(string template, T0 args0)
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(template, args0);
        }
    }
}