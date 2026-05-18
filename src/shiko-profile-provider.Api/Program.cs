using shiko_profile_provider.Infrastructure.Persistence;
using shiko_profile_provider.Infrastructure.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}

app.UseHttpsRedirection();


// Creating db
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.EnsureCreatedAsync();
}

app.Run();