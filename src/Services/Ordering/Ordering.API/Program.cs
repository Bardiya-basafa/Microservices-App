using Ordering.API;
using Ordering.Infrastructure;
using Ordering.Application;
using Ordering.Domain;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration);


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment()){
    await app.InitializeDatabase();
}

app.UseApiServices();

app.Run();
