using Blazored.LocalStorage;
using CpCinemaBlazor.ApiRequest.Model;
using System.Text.Json;

public class SingletoneUser
{
    private CurUser _currentUser;
    private readonly ILocalStorageService _localStorage;

    public event Action OnAuthStateChanged;

    public SingletoneUser(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public CurUser CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            NotifyAuthStateChanged();
            SaveUserToLocalStorage();
        }
    }

    public bool IsAuthenticated => _currentUser != null;

    public async Task Login(CurUser user)
    {
        CurrentUser = user;
        await SaveUserToLocalStorage();
    }

    public async Task Logout()
    {
        CurrentUser = null;
        await ClearLocalStorage();
        OnAuthStateChanged?.Invoke();
    }

    private async Task SaveUserToLocalStorage()
    {
        if (CurrentUser != null)
        {
            var serializedUser = JsonSerializer.Serialize(CurrentUser);
            await _localStorage.SetItemAsStringAsync("currentUser", serializedUser);
        }
    }

    private async Task ClearLocalStorage()
    {
        await _localStorage.RemoveItemAsync("currentUser");
    }

    public async Task LoadUserFromLocalStorage()
    {
        try
        {
            var userJson = await _localStorage.GetItemAsStringAsync("currentUser");

            if (!string.IsNullOrEmpty(userJson))
            {
                CurrentUser = JsonSerializer.Deserialize<CurUser>(userJson);
            }
        }
        catch (InvalidOperationException ex)
        {
            // Логируем ошибку, если доступ к LocalStorage невозможен
            Console.WriteLine($"Error loading user from LocalStorage: {ex.Message}");
        }
    }

    private void NotifyAuthStateChanged()
    {
        OnAuthStateChanged?.Invoke();
    }
}