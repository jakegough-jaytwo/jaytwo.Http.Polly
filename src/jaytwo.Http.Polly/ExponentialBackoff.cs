using System;
using System.Collections.Generic;
using System.Text;

namespace jaytwo.Http.Polly;

internal class ExponentialBackoff
{
    public const double DefaultMaxBackoffSeconds = 20;

    private static Random _random = new Random();

    public ExponentialBackoff(
        double maxBackoffSeconds = DefaultMaxBackoffSeconds,
        Func<double> randomNumberProvider = null)
    {
        MaxBackoffSeconds = maxBackoffSeconds;
        RandomNumberProvider = randomNumberProvider ?? _random.NextDouble;
    }

    protected double MaxBackoffSeconds { get; private set; }

    protected Func<double> RandomNumberProvider { get; private set; }

    public TimeSpan GetExponentialBackoffSleep(int retryCount)
    {
        // https://docs.aws.amazon.com/sdkref/latest/guide/feature-retry-behavior.html

        // seconds_to_sleep_i = min(b*r^i, MAX_BACKOFF)
        // b = random number within the range of: 0 <= b <= 1
        // r = 2
        // MAX_BACKOFF = 20 seconds

        var random = RandomNumberProvider.Invoke();

        var backoffSeconds = Math.Min(
            MaxBackoffSeconds,
            random * Math.Pow(2, retryCount));

        return TimeSpan.FromSeconds(backoffSeconds);
    }
}
