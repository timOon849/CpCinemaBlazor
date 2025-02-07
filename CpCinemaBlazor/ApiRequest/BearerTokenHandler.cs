using CpCinemaBlazor.ApiRequest.Services;
using System.Net.Http.Headers;

namespace CpCinemaBlazor.ApiRequest
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly LocalStorageService _localStorage;

        public BearerTokenHandler(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsync("authToken");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
