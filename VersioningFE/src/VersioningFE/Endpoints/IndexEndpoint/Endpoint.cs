using FastEndpoints.AspVersioning;

namespace VersioningFE.Endpoints;

public class IndexResponse
{
    public string Message { get; set; } = default!;
}

public class IndexEndpoint : EndpointWithoutRequest<IndexResponse>
{
    public override void Configure()
    {
        Get("/");
        Options(x => x
            .WithVersionSet(">>Orders<<")
            .MapToApiVersion(1.0)
            .MapToApiVersion(2.0));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(new IndexResponse
        {
            Message = $"Hello Fast Endpoints with version {HttpContext.Request.Headers["X-Api-Version"]}"
        }, ct);
    }
}