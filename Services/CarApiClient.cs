using System.Net.Http;
using System.Net.Http.Json;
using FribergCars.Models;

namespace FribergCars.Services
{
    public class CarApiClient
    {
        private readonly HttpClient _httpClient;

        public CarApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            var cars = await _httpClient.GetFromJsonAsync<List<Car>>("api/cars");
            return cars ?? new List<Car>();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Car>($"api/cars/{id}");
        }

        public async Task<Car?> CreateAsync(Car car)
        {
            var response = await _httpClient.PostAsJsonAsync("api/cars", car);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<Car>();
        }

        public async Task<bool> UpdateAsync(int id, Car car)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/cars/{id}", car);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/cars/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
