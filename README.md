# jaytwo.Http.Polly

<p align="center">
  <a href="https://jenkins.jaytwo.com/job/jaytwo.Http.Polly/job/master/" alt="Build Status (master)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=jaytwo.Http.Polly%2Fmaster&subject=build%20(master)" /></a>
  <a href="https://jenkins.jaytwo.com/job/jaytwo.Http.Polly/job/develop/" alt="Build Status (develop)">
    <img src="https://jenkins.jaytwo.com/buildStatus/icon?job=jaytwo.Http.Polly%2Fdevelop&subject=build%20(develop)" /></a>
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/jaytwo.Http.Polly/" alt="NuGet Package jaytwo.Http.Polly">
    <img src="https://img.shields.io/nuget/v/jaytwo.Http.Polly.svg?logo=nuget&label=jaytwo.Http.Polly" /></a>
  <a href="https://www.nuget.org/packages/jaytwo.Http.Polly/" alt="NuGet Package jaytwo.Http.Polly (beta)">
    <img src="https://img.shields.io/nuget/vpre/jaytwo.Http.Polly.svg?logo=nuget&label=jaytwo.Http.Polly" /></a>
</p>

## Installation

Add the NuGet package

```
PM> Install-Package jaytwo.Http.Polly
```

## Usage

This builds on the `IHttpClient` abstraction from the `jaytwo.Http` package.  This is meant for use with the `jaytwo.FluentHttp` package.

It provides two retry policies: 
* a default retry policy that retries 3 times, waiting 1, 3, and then 5 seconds between retries.
* an exponential backoff retry policy, based on the [published AWS client retry behavior](https://docs.aws.amazon.com/sdkref/latest/guide/feature-retry-behavior.html)


```csharp
// default retry policy
var httpClient = new HttpClient().Wrap().WithDefaultRetryPolicy();
```

```csharp
// exponential backoff retry policy
var httpClient = new HttpClient().Wrap().WithExponentialBackoffRetryPolicy();
```

---

Made with &hearts; by Jake
