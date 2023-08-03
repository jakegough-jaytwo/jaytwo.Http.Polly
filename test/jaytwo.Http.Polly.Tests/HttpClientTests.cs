using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace jaytwo.Http.Authentication.Tests;

public class HttpClientTests
{
    public const string HttpBinUrl = "http://httpbin.jaytwo.com/";

    private readonly HttpClient _httpClient;
    private readonly ITestOutputHelper _output;

    public HttpClientTests(ITestOutputHelper output)
    {
        _output = output;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(HttpBinUrl);
    }
}
