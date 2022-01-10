using App.Helpers;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using static App.Helpers.RandomHelper;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace App.Benchmarks;

[Config(typeof(BenchConfig))]
[BenchmarkCategory(nameof(BenchCategory.SerilogLogger))]
public class SerilogLoggerBench
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
        _logger = CreateSerilogLogger();
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

    private static ILogger CreateSerilogLogger()
    {
        const LogLevel minLogLevel = LogLevel.Information;
        const LogEventLevel minLogEventLevel = LogEventLevel.Information;

        var serilogLogger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Logger(c => c.Filter.ByIncludingOnly(e => e.Level >= minLogEventLevel).WriteTo.Console())
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(minLogLevel);
            builder.AddFilter(level => level >= minLogLevel);
            builder.AddSerilog(serilogLogger);
        });

        return new Logger<DefaultLoggerBench>(loggerFactory);
    }
}