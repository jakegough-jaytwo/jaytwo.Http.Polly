using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using jaytwo.Http.Wrappers;
using Polly;

namespace jaytwo.Http.Polly;

public class AsyncPolicyWrapper : DelegatingHttpClientWrapper, IHttpClient
{
    public AsyncPolicyWrapper(IHttpClient httpClient, IAsyncPolicy<HttpResponseMessage> policy)
        : base(httpClient)
    {
        Policy = policy;
    }

    public IAsyncPolicy<HttpResponseMessage> Policy { get; private set; }

    public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption? completionOption = null, CancellationToken? cancellationToken = null)
        => await Policy.ExecuteAsync(
            async ct => await base.SendAsync(request, completionOption, ct),
            cancellationToken ?? CancellationToken.None);
}
