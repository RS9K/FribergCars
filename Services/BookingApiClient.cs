using System.Net.Http;
using System.Net.Http.Json;
using FribergCars.Models;

namespace FribergCars.Services
{
    public class BookingApiClient
    {
        private readonly HttpClient _http;

        public BookingApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<Booking?> CreateAsync(Booking booking)
        {
            var response = await _http.PostAsJsonAsync(
                "api/bookings", booking);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<Booking>();
        }

        public async Task<List<Booking>> GetByCustomerIdAsync(int customerId)
        {
            var booking = await _http.GetFromJsonAsync<List<Booking>>(
                $"api/bookings/customer/{customerId}");
            return booking ?? new List<Booking>();
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<Booking>(
                $"api/bookings/{id}");
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            var bookings = await _http.GetFromJsonAsync<List<Booking>>(
                "api/bookings");
            return bookings ?? new List<Booking>();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync(
                $"api/bookings/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}

