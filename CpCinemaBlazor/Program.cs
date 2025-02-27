using Blazored.LocalStorage;
using Blazored.Toast;
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
builder.Services.AddSingleton<SingletoneUser>();


builder.Services.AddScoped<ApiRequestService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5005/") });
builder.Services.AddScoped<NotificationService>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005/");
});

builder.Services.AddSingleton<UserService>();

builder.Services.AddAuthorization();
builder.Services.AddBlazoredToast();
builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// «агрузка токена при старте приложени€
//using (var scope = app.Services.CreateScope())
//{
//    var localStorage = scope.ServiceProvider.GetRequiredService<LocalStorageService>();
//    SingletoneUser.Init(localStorage);
//    await SingletoneUser.LoadUserFromLocalStorage();
//}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorPages();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
