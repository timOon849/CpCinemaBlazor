using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CpCinemaBlazor.ApiRequest.Model;
using System.Text.Json;

public class SingletoneUser
{
    private CurUser _currentUser; // Текущий аутентифицированный пользователь
    private readonly ISessionStorageService _storageService; // Общий интерфейс для работы с хранилищем

    /// <summary>
    /// Событие, которое вызывается при изменении состояния аутентификации.
    /// </summary>
    public event Action OnAuthStateChanged;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="storageService">Служба для работы с локальным или сессионным хранилищем.</param>
    public SingletoneUser(ISessionStorageService storageService)
    {
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
    }

    /// <summary>
    /// Получить или установить текущего пользователя.
    /// </summary>
    public CurUser CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            NotifyAuthStateChanged(); // Уведомляем подписчиков об изменении состояния
            SaveUserToStorage(); // Сохраняем данные в хранилище
        }
    }

    /// <summary>
    /// Проверяет, аутентифицирован ли пользователь.
    /// </summary>
    public bool IsAuthenticated => _currentUser != null;

    /// <summary>
    /// Выполняет вход пользователя.
    /// </summary>
    /// <param name="user">Объект пользователя.</param>
    public async Task Login(CurUser user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        CurrentUser = user; // Устанавливаем текущего пользователя
        await SaveUserToStorage(); // Сохраняем данные в хранилище
    }

    /// <summary>
    /// Выполняет выход пользователя.
    /// </summary>
    public async Task Logout()
    {
        CurrentUser = null; // Очищаем текущего пользователя
        await ClearStorage(); // Очищаем хранилище
        OnAuthStateChanged?.Invoke(); // Уведомляем подписчиков
    }

    /// <summary>
    /// Загружает данные пользователя из хранилища.
    /// </summary>
    private async Task SaveUserToStorage()
    {
        if (CurrentUser != null)
        {
            var serializedUser = JsonSerializer.Serialize(CurrentUser);
            Console.WriteLine($"Saving user to storage: {serializedUser}");
            await _storageService.SetItemAsStringAsync("currentUser", serializedUser);
        }
    }

    public async Task LoadUserFromStorage()
    {
        try
        {
            var userJson = await _storageService.GetItemAsStringAsync("currentUser");
            Console.WriteLine($"Loading user from storage: {userJson}");

            if (!string.IsNullOrEmpty(userJson))
            {
                CurrentUser = JsonSerializer.Deserialize<CurUser>(userJson);
                NotifyAuthStateChanged();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user from storage: {ex.Message}");
        }
    }

    /// <summary>
    /// Очищает данные пользователя из хранилища.
    /// </summary>
    private async Task ClearStorage()
    {
        await _storageService.RemoveItemAsync("currentUser"); // Удаляем данные пользователя из хранилища
    }

    /// <summary>
    /// Уведомляет подписчиков о изменении состояния аутентификации.
    /// </summary>
    private void NotifyAuthStateChanged()
    {
        OnAuthStateChanged?.Invoke(); // Вызываем событие
    }
}