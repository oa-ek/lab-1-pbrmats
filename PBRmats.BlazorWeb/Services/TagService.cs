using PBRmats.Core.Entities;
using System.Net.Http.Json;

namespace PBRmats.BlazorWeb.Services
{
    public class TagService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7072";

        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            var items = await _httpClient.GetAsync(ApiUrl + "/api/TagApi");
            return await items.Content.ReadFromJsonAsync<List<Tag>>() ?? new List<Tag>();
        }

        public async Task<Tag> GetTagByIdAsync(int id)
        {
            var item = await _httpClient.GetAsync(ApiUrl + $"/api/TagApi/{id}");
            return await item.Content.ReadFromJsonAsync<Tag>() ?? new Tag();
        }

        public async Task<Tag> AddTagAsync(Tag tag)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl + "/api/TagApi", tag);
            response.EnsureSuccessStatusCode();

            return tag; // Return the created tag
        }

        public async Task UpdateTagAsync(int id, Tag tag)
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}/api/TagApi/{id}", tag);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTagAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(ApiUrl + $"/api/TagApi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
