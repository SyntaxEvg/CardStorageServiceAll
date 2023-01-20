using ServiceRegistration.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);
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

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IService, Service>();

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
var init =app.Services.GetRequiredService<IService>();
init.InitServices();//в качестве примера просто инициализируем сервисы
app.Run();
