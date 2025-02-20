//using Microsoft.JSInterop;
//using System.Text.Json;


//namespace CpCinemaBlazor.ApiRequest.Services
//{
//    public class LocalStorageService
//    {
//        private readonly IJSRuntime _jsRuntime;

//        public LocalStorageService(IJSRuntime jsRuntime)
//        {
//            _jsRuntime = jsRuntime;
//        }

//        // Сохранение значения в localStorage
//        public async Task SetItemAsync(string key, string value)
//        {
//            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
//        }

//        // Получение значения из localStorage
//        public async Task<string> GetItemAsync(string key)
//        {
//            // Проверяем, что _jsRuntime не null
//            if (_jsRuntime == null)
//            {
//                return null;
//            }
//            // Выполняем получение значения из localStorage
//            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
//        }
//    }
//}
