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
app.MapPost("/add-reminder", (
        string authorId, 
        string messageChannelId, 
        string messageContent
    ) => reminders.addReminder(authorId, messageChannelId, messageContent));

app.MapGet("/peek-reminder", () => 
{
    Reminder? r = reminders.peekReminder();
    // Return empty JSON on null
    if (r == null)
    {
        return new {};
    } 
    // Serialize Reminder
    return r.Serialize();
});

// Run app
app.Run();
