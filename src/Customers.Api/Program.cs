using Customers.Api.Endpoints.Internal;
using Customers.Api.Infrastructure.Data;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnectionFactory>(
    x=> new PostgresConnectionFactory(builder.Configuration.GetConnectionString("DefaultConnection")!));
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddHttpClient("GitHub", client=> 
{
    client.BaseAddress = new Uri(builder.Configuration["GitHub:ApiUrl"]!);
});

builder.Services.AddEndpoints<Program>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints<Program>();

var initializer = app.Services.GetRequiredService<DatabaseInitializer>();
await initializer.InitializeAsync();

app.Run();