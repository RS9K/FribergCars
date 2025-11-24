using System.Net.Http;
using System.Net.Http.Json;
using FribergCars.Models;

namespace FribergCars.Services
{
    public class AdminApiClient
    {
        private readonly HttpClient _http;
        public AdminApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<Administrator?> LoginAsync(string email, string password)
        {
            var body = new { email, password };
            var response = await _http.PostAsJsonAsync("api/admin/login", body);
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<Administrator>();
        }
    }
}
