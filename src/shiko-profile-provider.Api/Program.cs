using shiko_profile_provider.Api.Endpoints;
using shiko_profile_provider.Api.OpenApi;
using shiko_profile_provider.Api.Security;
using shiko_profile_provider.Infrastructure.Persistence;
using shiko_profile_provider.Infrastructure.Persistence.Contexts;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddProfileProviderOpenApi();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

// Creating db
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.EnsureCreatedAsync();
}

app.MapOpenApiEndpoints();
app.MapProfileEndpoints();

app.Run();