using System.Text;
using System.Text.Json;
using CpCinemaBlazor.ApiRequest.Model;
using Microsoft.Extensions.Logging;
using static CpCinemaBlazor.ApiRequest.Model.User;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CpCinemaBlazor.ApiRequest.Services;
using System;
using Azure;

namespace CpCinemaBlazor.ApiRequest
{
    public class ApiRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiRequestService> _logger;

        public ApiRequestService(HttpClient httpClient, ILogger<ApiRequestService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<UserDataShort>> GetUsersAsync()
        {
            var url = "api/UsersLogins/GetUsers";

            try
            {
                var response = await _httpClient.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(responseContent))
                {
                    _logger.LogWarning("Ответ от сервера пуст.");
                    return [];
                }

                var usersData = JsonSerializer.Deserialize<List<UserDataShort>>(responseContent, new JsonSerializerOptions //
                {
                    PropertyNameCaseInsensitive = true
                });
                 
                return usersData ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
                return [];
            }
        }

        public async void EditUserAsync(UserProd user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync("http://localhost:5005/api/UsersLogins/EditUser", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        public async void DeleteUserAsync(int Id)
        {
            try
            {   
                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5005/api/UsersLogins/DeleteUser/{Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        public async void CreateUserLoginAsync(AddUser user)
        {
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"http://localhost:5005/api/UsersLogins/CreateNewUserAndLogin", content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе");
            }
        }

        //eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi0JrQsNGC0Y8iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJVU0VSIiwibmJmIjoxNzM3NzMyNDUzLCJleHAiOjE3Mzc3MzQyNTMsImlzcyI6IkFQSVNlcnZlciIsImF1ZCI6IkJsYXpvckFwcCJ9.X-NkXeMxrG-EujaV4YzdpliXtXAiY16JckbOWdrgEqw

        
        public async Task<string> AuthorizationAsync(AuthUser auth)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5005/api/UsersLogins/Authorization", auth);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authorization failed: {errorContent}");
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Игнорировать регистр свойств
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Сопоставлять snake_case с CamelCase
            };
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, options);
            var token = tokenResponse.Token;

            return token;
        }

        public class TokenResponse
        {
            public bool Status { get; set; }
            public string Token { get; set; }
        }
    }
}

