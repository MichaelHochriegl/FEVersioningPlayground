using FastEndpoints.AspVersioning;

namespace VersioningFE.Endpoints.SecondEndpoint;

public class SecondEndpointV2 : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/second");
        Options(x => x
            .WithVersionSet(">>Orders<<")
            .MapToApiVersion(2.0));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync(HttpContext.Request.Headers["X-Api-Version"], ct);
    }
}