using Blazored.SessionStorage;
using CpCinemaBlazor.ApiRequest;
using CpCinemaBlazor.Components;
using Blazored.Toast; // Добавьте эту строку


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); // Добавляем поддержку Razor Pages
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Добавляем поддержку Razor Components
builder.Services.AddBlazoredToast();
// Регистрация Blazored.LocalStorage
// Регистрация SingletoneUser как scoped сервис
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<SingletoneUser>(sp =>
    new SingletoneUser(sp.GetRequiredService<ISessionStorageService>()));
builder.Services.AddScoped<ApiRequestService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5005/") });

//builder.Services.AddScoped<LocalStorageService>();

builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("http://localhost:5005/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Добавляем маршрутизацию
app.UseRouting();

// Добавляем защиту от подделки запросов
app.UseAntiforgery();

// Добавляем авторизацию
app.UseAuthorization();

// Настройка Razor Pages и Razor Components
app.MapRazorPages(); // Добавляем поддержку Razor Pages
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Добавляем поддержку Razor Components

app.Run();