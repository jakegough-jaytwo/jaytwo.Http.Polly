using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http.Polly;
using Polly;

namespace jaytwo.Http;

public static class IHttpClientExtensions
{
    public static IHttpClient WithPolicy(this IHttpClient httpClient, IAsyncPolicy<HttpResponseMessage> policy)
        => new AsyncPolicyWrapper(httpClient, policy);

    public static IHttpClient WithDefaultRetryPolicy(this IHttpClient httpClient)
        => WithPolicy(httpClient, Policies.DefaultRetryPolicy);

    public static IHttpClient WithExponentialBackoffRetryPolicy(this IHttpClient httpClient, int retryCount = 3, double maxBackoffSeconds = ExponentialBackoff.DefaultMaxBackoffSeconds)
        => WithPolicy(httpClient, Policies.GenerateExponentialBackoffRetryPolicy(retryCount, maxBackoffSeconds));
}
