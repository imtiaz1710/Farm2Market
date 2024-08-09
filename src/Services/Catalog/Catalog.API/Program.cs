var builder = WebApplication.CreateBuilder(args);
// Add Services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

app.MapCarter();

app.Run();
