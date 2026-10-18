//this creates a builder to configure the application.
//Know this : our application needs to be configured and we are creating a builder.
using POS.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//registering Controller Services
builder.Services.AddControllers();

//Registering ProductRepository services with AddScope.
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//added swagger code.

//this builds the configured web application.
var app = builder.Build();

//making controllers route accessible.
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

