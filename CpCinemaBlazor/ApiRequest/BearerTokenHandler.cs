//using CpCinemaBlazor.ApiRequest.Services;
//using System.Net.Http.Headers;

//namespace CpCinemaBlazor.ApiRequest
//{
//    public class BearerTokenHandler : DelegatingHandler
//    {
//        private readonly LocalStorageService _localStorage;

//        public BearerTokenHandler(LocalStorageService localStorage)
//        {
//            _localStorage = localStorage;
//        }

//        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            // Проверяем, что _localStorage не является null
//            if (_localStorage == null)
//            {
//                return await base.SendAsync(request, cancellationToken);
//            }
//            else
//            {
//                // Получаем токен из локального хранилища
//                var token = await _localStorage.GetItemAsync("authToken");

//                // Добавляем заголовок авторизации, если токен существует
//                if (!string.IsNullOrEmpty(token))
//                {
//                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                }

//                // Выполняем следующий обработчик в цепочке
//                return await base.SendAsync(request, cancellationToken);
//            }
            
//        }
//    }
//}
