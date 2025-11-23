using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using FribergCars.Models;

namespace FribergCars.Services
{
    public class CustomerApiClient
    {
        private readonly HttpClient _httpClient;

        public CustomerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Customer?> RegisterAsync(Customer customer)
        {
            var result = await _httpClient.PostAsJsonAsync("api/customers", customer);
            if (!result.IsSuccessStatusCode)
                return null;
            return await result.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task<Customer?> LoginAsync(string email, string password)
        {
            var body = new { email, password };
            var result = await _httpClient.PostAsJsonAsync(
                "api/customers/login", body);

            if (!result.IsSuccessStatusCode)
                return null;
            return await result.Content.ReadFromJsonAsync<Customer>();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>(
                $"api/customers/{id}");
        }
        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _httpClient.GetFromJsonAsync<List<Customer>>(
                "api/customers");
            return customers ?? new List<Customer>();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/customers/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}