using Gateway.Middlewares;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.OpenApi.Models;
using Net7.Core;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle




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


//加载Ocelot
builder.Configuration.AddJsonFile($"Ocelot.{EnvironmentName}.Api.json", optional: false, reloadOnChange: true);
//如开启熔断必须开启缓存 否则回失败
builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly()
    .AddCacheManager(settings => { settings.WithDictionaryHandle(); })
    //.AddConsul()
    ;


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
System.Console.WriteLine("Gateway start");
app.UseSerilogSetup();
app.UseMiddleware<ReqIgnoreMidd>();
app.UseMiddleware<ExceptionMidd>();
app.UseMiddleware<ReqResLogMidd>();

app.UseOcelot().Wait();
app.Run();