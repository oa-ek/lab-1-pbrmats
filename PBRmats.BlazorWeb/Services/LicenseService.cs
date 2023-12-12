using PBRmats.Core.Entities;
using System.Net.Http.Json;

namespace PBRmats.BlazorWeb.Services
{
    public class LicenseService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7072";

        public LicenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<License>> GetLicensesAsync()
        {
            var items = await _httpClient.GetAsync(ApiUrl + "/api/LicenseApi");
            return await items.Content.ReadFromJsonAsync<List<License>>() ?? new List<License>();
        }

        public async Task<License> GetLicenseByIdAsync(int id)
        {
            var item = await _httpClient.GetAsync(ApiUrl + $"/api/LicenseApi/{id}");
            return await item.Content.ReadFromJsonAsync<License>() ?? new License();
        }

        public async Task<License> AddLicenseAsync(License license)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl + "/api/LicenseApi", license);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<License>() ?? new License();
        }

        public async Task UpdateLicenseAsync(int id, License license)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/api/LicenseApi/{id}", license);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteLicenseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(ApiUrl + $"/api/LicenseApi/{id.ToString()}");
            response.EnsureSuccessStatusCode();
        }
    }
}
