using Microsoft.OpenApi.Models;
using ReminderUtilities;

// Set up builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
   {
       c.SwaggerDoc("v1", new OpenApiInfo { Title = "RemindMe API", Description = "A Backend API for a RemindMe Discord Bot.", Version = "v1" });
   });

// Set up app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
   {
     c.SwaggerEndpoint("/swagger/v1/swagger.json", "RemindMe API V1");
   });

// Set up ReminderQueue
var reminders = new ReminderQueue();

// Add endpoints
app.MapGet("/", () => "Hello World!");
app.MapGet("/add-reminder", () => "Hello World!");

// Run app
app.Run();
