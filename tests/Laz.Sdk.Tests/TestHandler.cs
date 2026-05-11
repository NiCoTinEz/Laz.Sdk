namespace Laz.Sdk.Tests;

/// <summary>Captures the outgoing request and returns a canned response.</summary>
internal sealed class TestHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> responder) : HttpMessageHandler
{
    public HttpRequestMessage? LastRequest { get; private set; }
    public string? LastRequestBody { get; private set; }

    public TestHandler(HttpResponseMessage canned)
        : this(_ => Task.FromResult(canned)) { }

    public TestHandler(string jsonBody, System.Net.HttpStatusCode status = System.Net.HttpStatusCode.OK)
        : this(new HttpResponseMessage(status) { Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json") }) { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        if (request.Content is not null)
        {
            LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        }
        return await responder(request).ConfigureAwait(false);
    }
}
