using ServiceRegistration.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks();
var url = builder.Configuration["Consul:clientAddress"];
var IP = builder.Configuration["serviceInfo:ip"];
var port = builder.Configuration["serviceInfo:port"];
var HealthCheck = builder.Configuration["serviceInfo:healthCheckAddress"];
var urlsUse = $"http://{IP}:{port}";
builder.WebHost.UseUrls(urlsUse);
builder.Services.UseConul(url, "TestDC");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks(HealthCheck);//.RequireAuthorization(); если нужна проверка авторизация 
app.Run();
