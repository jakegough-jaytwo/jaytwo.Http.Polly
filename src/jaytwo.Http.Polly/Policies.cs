using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Polly;

namespace jaytwo.Http.Polly;

public class Policies
{
    public static IList<HttpStatusCode> RetryableStatusCodes { get; } = new HttpStatusCode[]
    {
        // codes taken from: https://docs.aws.amazon.com/sdkref/latest/guide/feature-retry-behavior.html

        HttpStatusCode.RequestTimeout,
        (HttpStatusCode)429, // Too Many Requests
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout,
        (HttpStatusCode)509, // Bandwidth Limit Exceeded
    }.ToList().AsReadOnly();

    public static IAsyncPolicy<HttpResponseMessage> DefaultRetryPolicy { get; } = GenerateDefaultRetryPolicy();

    public static IAsyncPolicy<HttpResponseMessage> GenerateDefaultRetryPolicy(IList<HttpStatusCode> retryableStatusCodes = default)
        => GenerateDefaultRetryPolicyBuilder(retryableStatusCodes ?? RetryableStatusCodes)
        .WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(0.5),
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(2),
        })
        .WithPolicyKey("DefaultRetryPolicy");

    public static IAsyncPolicy<HttpResponseMessage> GenerateExponentialBackoffRetryPolicy(int retryCount = 3, int maxBackoffSeconds = 25, IList<HttpStatusCode> retryableStatusCodes = default)
        => GenerateDefaultRetryPolicyBuilder(retryableStatusCodes ?? RetryableStatusCodes)
        .WaitAndRetryAsync(retryCount, x => ExponentialBackoff.GetExponentialBackoffSleep(x, maxBackoffSeconds))
        .WithPolicyKey("ExponentialBackoffRetryPolicy");

    public static PolicyBuilder<HttpResponseMessage> GenerateDefaultRetryPolicyBuilder(IList<HttpStatusCode> retryableStatusCodes)
        => Policy.HandleResult<HttpResponseMessage>(response => retryableStatusCodes.Contains(response.StatusCode))
        .Or<HttpRequestException>()
        .Or<IOException>()
        .OrInner<IOException>();
}
