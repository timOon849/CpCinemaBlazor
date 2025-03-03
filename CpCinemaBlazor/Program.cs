using Blazored.SessionStorage;
using CpCinemaBlazor.ApiRequest;
using CpCinemaBlazor.Components;
using Blazored.Toast; // �������� ��� ������


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); // ��������� ��������� Razor Pages
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // ��������� ��������� Razor Components
builder.Services.AddBlazoredToast();
// ����������� Blazored.LocalStorage
// ����������� SingletoneUser ��� scoped ������
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

// ��������� �������������
app.UseRouting();

// ��������� ������ �� �������� ��������
app.UseAntiforgery();

// ��������� �����������
app.UseAuthorization();

// ��������� Razor Pages � Razor Components
app.MapRazorPages(); // ��������� ��������� Razor Pages
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // ��������� ��������� Razor Components

app.Run();