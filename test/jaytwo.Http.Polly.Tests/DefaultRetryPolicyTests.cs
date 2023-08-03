using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Polly;
using Xunit;

namespace jaytwo.Http.Polly.Tests;

public class DefaultRetryPolicyTests
{
    [Fact]
    public async Task DefaultRetryPolicy_retries_4_times()
    {
        // Arrange
        using var request = new HttpRequestMessage();

        var mockClient = new Mock<IHttpClient>();

        mockClient
            .Setup(x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.BadGateway });

        var wrapped = new AsyncPolicyWrapper(mockClient.Object, Policies.DefaultRetryPolicy);

        // Act
        using var response = await wrapped.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);

        mockClient.Verify(
            x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()),
            Times.Exactly(4));
    }

    [Fact]
    public async Task ExponentialBackoffRetryPolicy_retries_4_times()
    {
        // Arrange
        using var request = new HttpRequestMessage();

        var mockClient = new Mock<IHttpClient>();

        mockClient
            .Setup(x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = HttpStatusCode.BadGateway });

        var wrapped = mockClient.Object.WithExponentialBackoffRetryPolicy(3, 1);

        // Act
        using var response = await wrapped.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadGateway, response.StatusCode);

        mockClient.Verify(
            x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()),
            Times.Exactly(4));
    }
}
