using PBRmats.Core.Entities;
using System.Net.Http.Json;

namespace PBRmats.BlazorWeb.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7072";

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var items = await _httpClient.GetAsync(ApiUrl + "/api/CategoryApi");
            return await items.Content.ReadFromJsonAsync<List<Category>>() ?? new List<Category>();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var item = await _httpClient.GetAsync(ApiUrl + $"/api/CategoryApi/{id}");
            return await item.Content.ReadFromJsonAsync<Category>() ?? new Category();
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl + "/api/CategoryApi", category);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Category>() ?? new Category();
        }

        public async Task UpdateCategoryAsync(int id, Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/api/CategoryApi/{id}", category);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(ApiUrl + $"/api/CategoryApi/{id.ToString()}");
            response.EnsureSuccessStatusCode();
        }
    }
}
