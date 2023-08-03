using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Polly;
using Xunit;

namespace jaytwo.Http.Polly.Tests;

public class IHttpClientExtensionsTests
{
    [Fact]
    public void WithPolicy_assigns_expected_policy()
    {
        // Arrange
        var mockPolicy = new Mock<IAsyncPolicy<HttpResponseMessage>>();
        var mockClient = new Mock<IHttpClient>();

        // Act
        var wrapped = mockClient.Object.WithPolicy(mockPolicy.Object);

        // Assert
        var typed = Assert.IsType<AsyncPolicyWrapper>(wrapped);
        Assert.Same(mockPolicy.Object, typed.Policy);
    }

    [Fact]
    public void WithDefaultRetryPolicy_assigns_DefaultRetryPolicy()
    {
        // Arrange
        var mockClient = new Mock<IHttpClient>();

        // Act
        var wrapped = mockClient.Object.WithDefaultRetryPolicy();

        // Assert
        var typed = Assert.IsType<AsyncPolicyWrapper>(wrapped);
        Assert.Same(Policies.DefaultRetryPolicy, typed.Policy);
    }

    [Fact]
    public void WithExponentialBackoffRetryPolicy_assigns_ExponentialRetryPolicy()
    {
        // Arrange
        var mockClient = new Mock<IHttpClient>();

        // Act
        var wrapped = mockClient.Object.WithExponentialBackoffRetryPolicy();

        // Assert
        var typed = Assert.IsType<AsyncPolicyWrapper>(wrapped);
        Assert.Equal("ExponentialBackoffRetryPolicy", typed.Policy.PolicyKey);
    }
}
