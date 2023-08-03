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

public class PollyWrapperTests
{
    [Fact]
    public async Task AsyncPolicyWrapper_invokces_policy()
    {
        // Arrange
        using var request = new HttpRequestMessage();

        var mockClient = new Mock<IHttpClient>();
        var policy = Policy
            .HandleResult<HttpResponseMessage>(x => true)
            .FallbackAsync(x => Task.FromResult(new HttpResponseMessage() { StatusCode = (HttpStatusCode)418 }));

        mockClient
            .Setup(x => x.SendAsync(request, It.IsAny<HttpCompletionOption?>(), It.IsAny<CancellationToken?>()))
            .ReturnsAsync((HttpResponseMessage)null);

        var wrapped = new AsyncPolicyWrapper(mockClient.Object, policy);

        // Act
        using var response = await wrapped.SendAsync(request);

        // Assert
        Assert.Equal((HttpStatusCode)418, response.StatusCode);
    }
}
