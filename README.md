# LoggingBenchDemo
```
Benchmarking ways of logging messages with or without arguments
```

In this demo, i m using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library in order to benchmark various ways of logging messages with or without arguments.
>
> :one: `DefaultLoggerBench` : a bench based on [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) provided by microsoft
>
> :two: `DefaultLoggerVariantBench` : a bench based on a wrapper around [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) provided by microsoft
>
> :three: `SerilogLoggerBench` : a bench based on a [default logger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) which is configured with serilog logger 
>
> :four: `SerilogLoggerVariantBench` : a bench based on directly serilog logger interface and implementation
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
|                                   Method | Length |       Mean |     Error |    StdDev |       Min |        Max | Rank |  Gen 0 | Allocated |
|----------------------------------------- |------- |-----------:|----------:|----------:|----------:|-----------:|-----:|-------:|----------:|
|       NoParamsTraceLoggingUsingIsEnabled |     50 |   8.483 ns | 0.1375 ns | 0.2058 ns |  8.104 ns |   8.885 ns |    1 |      - |         - |
|   NoParamsTraceLoggingUsingLoggerMessage |     50 |   9.477 ns | 0.2135 ns | 0.4687 ns |  8.826 ns |  10.659 ns |    2 |      - |         - |
|     WithParamsTraceLoggingUsingIsEnabled |     50 |   9.615 ns | 0.2715 ns | 0.7703 ns |  8.486 ns |  11.815 ns |    2 |      - |         - |
|          NoParamsTraceLoggingUsingDefine |     50 |  11.290 ns | 0.2526 ns | 0.5217 ns | 10.120 ns |  12.589 ns |    3 |      - |         - |
| WithParamsTraceLoggingUsingLoggerMessage |     50 |  15.697 ns | 0.2942 ns | 0.5738 ns | 14.785 ns |  17.296 ns |    4 |      - |         - |
|        WithParamsTraceLoggingUsingDefine |     50 |  18.059 ns | 0.3623 ns | 0.8398 ns | 16.835 ns |  20.137 ns |    5 |      - |         - |
|                     NoParamsTraceLogging |     50 |  30.731 ns | 0.6359 ns | 1.2098 ns | 29.241 ns |  33.273 ns |    6 |      - |         - |
|                   WithParamsTraceLogging |     50 | 111.271 ns | 2.9788 ns | 8.7831 ns | 99.501 ns | 134.020 ns |    7 | 0.0134 |      56 B |
```

>
**`References`** :
>
> :zap: [You are doing .NET logging wrong. Let's fix it](https://www.youtube.com/watch?v=bnVfrd3lRv8)
>
> :za: [High-performance logging with LoggerMessage](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/loggermessage)

>
**`Tools`** : vs22, net 6.0, serilog