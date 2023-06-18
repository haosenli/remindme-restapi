using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "RemindMe API", Description = "A Backend API for a RemindMe Discord Bot.", Version = "v1" });
   });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
