using ChatApplication.Client.Pages;
using ChatApplication.Components;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System;
using ChatApplication.Data;
using System.Diagnostics;
using ChatApplication.Services;


var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    new[] { "application/octet-stream" });
});

var sqlConnection = builder.Configuration["Chat:SqlDb"];

builder.Services.AddSqlServer<ApplicationDbContext>(sqlConnection, 
    options => options.EnableRetryOnFailure());

builder.Services.AddScoped<ChatService>();

var app = builder.Build();

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

app.MapHub<ChatHub>("/chathub");

app.Run();
