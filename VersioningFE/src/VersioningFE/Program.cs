using System.Text.Json;

using Asp.Versioning;
using Asp.Versioning.Conventions;

using FastEndpoints.AspVersioning;
using FastEndpoints.ClientGen;

using VersioningFE.Extensions;
using FastEndpoints.Swagger;

VersionSets.CreateApi(">>Orders<<", v => v
    .HasApiVersion(1.0)
    .HasApiVersion(2.0));

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
    options.AllowSynchronousIO = false;
});


builder.Host.UseConsoleLifetime(options => options.SuppressStatusMessages = true);
builder.Services.AddApiVersioning();
builder.Services.AddProblemDetails();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddFastEndpoints(options => { options.SourceGeneratorDiscoveredTypes = new Type[] { }; })
    .AddVersioning(o =>
{
    o.DefaultApiVersion = new(1.0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
});

builder.Services.SwaggerDocument(o =>
    {
        o.DocumentSettings = x =>
        {
            x.DocumentName = "v1";
            x.ApiVersion(new(1.0));
        };
        o.AutoTagPathSegmentIndex = 0;
    })
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = x =>
        {
            x.DocumentName = "v2";
            x.ApiVersion(new(2.0));
        };
        o.AutoTagPathSegmentIndex = 0;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultExceptionHandler();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
    options.Errors.ResponseBuilder = (errors, _, _) => errors.ToResponse();
    options.Errors.StatusCode = StatusCodes.Status422UnprocessableEntity;
    options.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3(x => x.ConfigureDefaults());
}
await app.GenerateClientsAndExitAsync(
    documentName: "v1", //must match doc name above
    destinationPath: builder.Environment.ContentRootPath,
    csSettings: c => c.ClassName = "ApiClient",
    tsSettings: null);

await app.RunAsync();

public partial class Program
{
}