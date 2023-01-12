using ClientServiceProtos;
using Web.TestGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseWhen(ctx =>
    ctx.Request.ContentType != "application/grpc", buider => buider.UseHttpLogging()
);
app.MapControllers();

app.UseEndpoints(end =>
{
    end.MapGrpcService<CardServices>();
});

app.Run();
