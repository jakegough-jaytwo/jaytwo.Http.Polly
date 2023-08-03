using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        var exponentialBackoff = new ExponentialBackoff(maxBackoffSeconds, () => random);

        // Act
        var actual = exponentialBackoff.GetExponentialBackoffSleep(retryCount);

        // Assert
        Assert.Equal(expected, actual.TotalSeconds);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task ExponentialBackoffRetryPolicy_retries_n_times(int retryCount)
    {
        // Arrange
        using var request = new HttpRequestMessage();

        var mockClient = new Mock<IHttpClient>();

        mockClient
            .Setup(x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.BadGateway });

        var wrapped = mockClient.Object.WithExponentialBackoffRetryPolicy(retryCount, 1);

        // Act
        using var response = await wrapped.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);

        mockClient.Verify(
            x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()),
            Times.Exactly(retryCount + 1));
    }
}
