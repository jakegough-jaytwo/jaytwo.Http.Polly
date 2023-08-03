using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace jaytwo.Http.Polly.Tests;

public class ExponentialBackoffTests
{
    [Theory]
    [InlineData(100, 1, 1, 1)]
    [InlineData(1, 1, 1, 1)]
    [InlineData(1, 100, 1, 2)]
    [InlineData(1, 100, 0.25, 0.5)]
    [InlineData(3, 100, 1, 8)]
    public void GetExponentialBackoffSleep_returns_expected(int retryCount, double maxBackoffSeconds, double random, double expected)
    {
        // Arrange

        // Act
        var actual = ExponentialBackoff.GetExponentialBackoffSleep(retryCount, maxBackoffSeconds, random);

        // Assert
        Assert.Equal(expected, actual.TotalSeconds);
    }
}
