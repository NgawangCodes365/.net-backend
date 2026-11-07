var builder = WebApplication.CreateBuilder(args);

// Register services used by the HTTP pipeline.
// EndpointsApiExplorer + SwaggerGen power interactive API docs in Development.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Development-only OpenAPI UI for exploring endpoints as we add them.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// No domain endpoints yet. Inventory features will be added deliberately,
// after architecture and data-model decisions are approved.

app.Run();
