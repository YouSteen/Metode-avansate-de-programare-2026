using OrderProcessing.Api.Endpoints;
using OrderProcessing.Api.Services;
using OrderProcessing.Api.Validation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddSingleton(ValidationChainFactory.Build());
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapOrderEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.MapFallbackToFile("index.html");

app.Run();
