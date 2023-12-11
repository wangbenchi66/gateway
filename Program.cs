using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IGeekFan.AspNetCore.Knife4jUI;
using Net7.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle




//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
          c.SwaggerDoc("v1", new OpenApiInfo
          {
              Title = "Gateway api",
              Version = "v1"
          })
          );

var EnvironmentName = builder.Environment.EnvironmentName;
//根据环境变量加载配置文件
builder.Configuration.AddJsonFile($"appsettings.{EnvironmentName}.json", optional: false, reloadOnChange: true);



builder.Configuration.AddJsonFile($"Ocelot.{EnvironmentName}.Api.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);


builder.Services.AddEndpointsApiExplorer();

var configuration = builder.Configuration;
builder.Host.AddSerilogHost(builder.Services, configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;
    c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "webapi");
});
app.UseKnife4UI(c =>
{
    c.SwaggerEndpoint("../api/swagger/v1/swagger.json", "api");
    c.RoutePrefix = "k4j"; // serve the UI at root
});

app.UseSerilogSetup();

app.UseOcelot().Wait();
app.Run();