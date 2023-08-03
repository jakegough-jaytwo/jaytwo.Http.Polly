using System;
using System.Collections.Generic;
using System.Text;

namespace jaytwo.Http.Polly;

internal class ExponentialBackoff
{
    private static Random _random = new Random();

    public static TimeSpan GetExponentialBackoffSleep(int retryCount, double maxBackoffSeconds)
        => GetExponentialBackoffSleep(retryCount, maxBackoffSeconds, _random.NextDouble());

    public static TimeSpan GetExponentialBackoffSleep(int retryCount, double maxBackoffSeconds, double random)
    {
        if (random < 0 || random > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(random), "Value must be between 0 and 1");
        }

        // https://docs.aws.amazon.com/sdkref/latest/guide/feature-retry-behavior.html

        // seconds_to_sleep_i = min(b*r^i, MAX_BACKOFF)
        // b = random number within the range of: 0 <= b <= 1
        // r = 2
        // MAX_BACKOFF = 20 seconds

        var backoffSeconds = Math.Min(
            maxBackoffSeconds,
            random * Math.Pow(2, retryCount));

        return TimeSpan.FromSeconds(backoffSeconds);
    }
}
