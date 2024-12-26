using ChatApplication.Client.Pages;
using ChatApplication.Components;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System;
using ChatApplication.Data;
using System.Diagnostics;
using ChatApplication.Services;
using ChatApplication.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Use user secrets in development
if (builder.Environment.IsDevelopment()) 
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
// Add SignalR service
builder.Services.AddSignalR(); 

// Add response compression
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    new[] { "application/octet-stream" });
});

// String for connection to db
var sqlConnection = builder.Configuration["ConnectionStrings:Chat:SqlDb"];

builder.Services.AddSqlServer<ApplicationDbContext>(sqlConnection,
    options => options.EnableRetryOnFailure());

// Add ChatService
builder.Services.AddScoped<ChatService>();

var app = builder.Build();

// Add migration to db in Azure
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ChatApplication.Client._Imports).Assembly);

// Add SignalR hub
app.MapHub<ChatHub>("/chathub");

app.Run();
