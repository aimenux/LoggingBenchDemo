using Microsoft.Extensions.Logging;

namespace App.Helpers
{
    public static partial class LoggerMessageHelper
    {
        private const string Message = "Lorem ipsum dolor sit amet consectetuer adipiscin";

        [LoggerMessage(100, LogLevel.Trace, Message)]
        public static partial void NoParamsLogTrace(ILogger logger);

        [LoggerMessage(0, LogLevel.Trace, $"{Message} {{number}}")]
        public static partial void WithParamsLogTrace(ILogger logger, int number);
    }
}
