[![.NET](https://github.com/aimenux/LoggingBenchDemo/actions/workflows/ci.yml/badge.svg)](https://github.com/aimenux/LoggingBenchDemo/actions/workflows/ci.yml)

# LoggingBenchDemo
```
Benchmarking ways of logging messages with or without arguments
```

In this demo, i m using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library in order to benchmark various ways of logging messages with or without arguments.
>
> :one: `DefaultLoggerBench` : a bench based on [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) provided by microsoft
>
> :two: `DefaultLoggerVariantBench` : a bench based on a custom wrapper around [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger)
>
> :three: `SerilogLoggerBench` : a bench based on a [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) configured with [serilog logger](https://github.com/serilog/serilog)
>
> :four: `SerilogLoggerVariantBench` : a bench based directly on [serilog logger](https://github.com/serilog/serilog)
>

In order to run benchmarks, type this command in your favorite terminal :
>
> :writing_hand: `.\App.exe --filter *DefaultLoggerBench*`
>
> :writing_hand: `.\App.exe --filter *DefaultLoggerVariantBench*`
>
> :writing_hand: `.\App.exe --filter *SerilogLoggerBench*`
>
> :writing_hand: `.\App.exe --filter *SerilogLoggerVariantBench*`
>

```
|                                   Method | Length |       Mean |        Min |        Max | Rank |  Gen 0 | Allocated |
|----------------------------------------- |------- |-----------:|-----------:|-----------:|-----:|-------:|----------:|
|       NoParamsTraceLoggingUsingIsEnabled |     50 |   8.150 ns |   7.863 ns |   8.918 ns |    1 |      - |         - |
|     WithParamsTraceLoggingUsingIsEnabled |     50 |   9.540 ns |   8.514 ns |  11.915 ns |    2 |      - |         - |
|   NoParamsTraceLoggingUsingLoggerMessage |     50 |   9.575 ns |   9.032 ns |  10.744 ns |    2 |      - |         - |
|          NoParamsTraceLoggingUsingDefine |     50 |  10.493 ns |  10.046 ns |  11.195 ns |    3 |      - |         - |
| WithParamsTraceLoggingUsingLoggerMessage |     50 |  17.202 ns |  15.309 ns |  21.691 ns |    4 |      - |         - |
|        WithParamsTraceLoggingUsingDefine |     50 |  17.657 ns |  16.493 ns |  20.044 ns |    5 |      - |         - |
|                     NoParamsTraceLogging |     50 |  31.430 ns |  30.491 ns |  33.146 ns |    6 |      - |         - |
|                   WithParamsTraceLogging |     50 | 113.599 ns | 100.364 ns | 141.127 ns |    7 | 0.0134 |      56 B |
```

**As you can see, the default logger is not optimized unless you use [IsEnabled](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger.isenabled), [Define](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggermessage.define) or [LoggerMessage](https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator) in order to eliminates the issue of object[] allocation and unnecessary calculations.**

>
**`References`** :
>
> :zap: [You are doing .NET logging wrong](https://www.youtube.com/watch?v=bnVfrd3lRv8)
>
> :zap: [High-Performance logging with LoggerMessage](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage)
>
> :zap: [Compile-time logging source generation](https://docs.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)

>
**`Tools`** : vs22, net 6.0, serilog