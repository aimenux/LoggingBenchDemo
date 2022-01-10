using App.Helpers;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using static App.Helpers.RandomHelper;

namespace App.Benchmarks;

[Config(typeof(BenchConfig))]
[BenchmarkCategory(nameof(BenchCategory.DefaultLogger))]
public class DefaultLoggerBench
{
    private string _message;
    private string _template;
    private ILogger _logger;

    private Action<ILogger, Exception> _noParamsLoggerMessage;
    private Action<ILogger, int, Exception> _withParamsLoggerMessage;

    private static readonly EventId NoParams = new(100, nameof(NoParams));
    private static readonly EventId WithParams = new(200, nameof(WithParams));

    public static int RandomNumber => Random.Shared.Next();

    [Params(50)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _message = RandomString(Length);
        _template = $"{_message} {{number}}";
        _logger = CreateDefaultLogger();
        _noParamsLoggerMessage = LoggerMessage.Define(LogLevel.Trace, NoParams, _message);
        _withParamsLoggerMessage = LoggerMessage.Define<int>(LogLevel.Trace, WithParams, _template);
    }

    [Benchmark]
    public void NoParamsTraceLogging()
    {
        _logger.LogTrace(_message);
    }

    [Benchmark]
    public void NoParamsTraceLoggingUsingIsEnabled()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(_message);
        }
    }

    [Benchmark]
    public void NoParamsTraceLoggingUsingLoggerMessage()
    {
        LoggerMessageHelper.NoParamsLogTrace(_logger);
    }

    [Benchmark]
    public void NoParamsTraceLoggingUsingDefine()
    {
        _noParamsLoggerMessage(_logger, null);
    }

    [Benchmark]
    public void WithParamsTraceLogging()
    {
        _logger.LogTrace(_template, RandomNumber);
    }

    [Benchmark]
    public void WithParamsTraceLoggingUsingIsEnabled()
    {
        if (_logger.IsEnabled(LogLevel.Trace))
        {
            _logger.LogTrace(_template, RandomNumber);
        }
    }

    [Benchmark]
    public void WithParamsTraceLoggingUsingDefine()
    {
        _withParamsLoggerMessage(_logger, RandomNumber, null);
    }

    [Benchmark]
    public void WithParamsTraceLoggingUsingLoggerMessage()
    {
        LoggerMessageHelper.WithParamsLogTrace(_logger, RandomNumber);
    }

    private static ILogger CreateDefaultLogger()
    {
        const LogLevel minLogLevel = LogLevel.Information;

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minLogLevel);
            builder.AddFilter(level => level >= minLogLevel);
            builder.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss:fff] ");
        });

        return new Logger<DefaultLoggerBench>(loggerFactory);
    }
}