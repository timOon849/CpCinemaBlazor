using CpCinemaBlazor.ApiRequest.Model;
using CpCinemaBlazor.ApiRequest.Services;

namespace CpCinemaBlazor.ApiRequest
{
    public static class SingletoneUser
    {
        public static CurUser curuser;
        private static LocalStorageService _localStorage;

        public static void Init(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public static async Task InitUser(CurUser user)
        {
            curuser = user;

            // Сохраняем токен в локальном хранилище
            if (!string.IsNullOrEmpty(user.Token) && _localStorage != null)
            {
                await _localStorage.SetItemAsync("authToken", user.Token);
            }
        }

        public static string GetToken()
        {
            return curuser?.Token;
        }

        public static void exitUser()
        {
            curuser = null;
        }
    }
}
