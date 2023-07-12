using FastEndpoints.AspVersioning;

namespace VersioningFE.Endpoints.SecondEndpoint;

public class SecondEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        Get("/second");
        Options(x => x
            .WithVersionSet(">>Orders<<")
            .MapToApiVersion(1.0)
            .MapToApiVersion(2.0));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendOkAsync("OLD", ct);
    }
}