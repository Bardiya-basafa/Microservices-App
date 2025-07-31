var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();

builder.Services.AddMediatR(configuration: config => {
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(options => {
    options.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssemblies([typeof(Program).Assembly]);

var app = builder.Build();

app.MapCarter();
app.Run();
