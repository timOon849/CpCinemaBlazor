using CpCinemaBlazor.ApiRequest;
using CpCinemaBlazor.ApiRequest.Services;
using CpCinemaBlazor.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<ApiRequestService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5005/") });

builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005/");
})
.AddHttpMessageHandler<BearerTokenHandler>();

// Регистрация BearerTokenHandler
builder.Services.AddTransient<BearerTokenHandler>(sp =>
{
    var localStorage = sp.GetRequiredService<LocalStorageService>();
    return new BearerTokenHandler(localStorage);
});

builder.Services.AddSingleton<UserService>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
