using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using PManager.API;

namespace PManagerApp.Core
{

    public class PasswordService : IPasswordService
    {
        private readonly HttpClient _httpClient;

        public PasswordService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/") // Адрес вашего API
            };
        }

        public async Task SavePasswordAsync(PasswordModel password)
        {
            var response = await _httpClient.PostAsJsonAsync("query/save", password);
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> GetPasswordAsync(string login)
        {
            var response = await _httpClient.GetAsync($"query/get/{login}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string>(json);
            }
            return null;
        }

        public async Task ChangePasswordAsync(PasswordModel password)
        {
            var response = await _httpClient.PostAsJsonAsync("query/change", password);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<PasswordModel>> GetAllPasswordsAsync()
        {
            var response = await _httpClient.GetAsync("query/get/list");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PasswordModel>>(json);
            }
            return new List<PasswordModel>();
        }
    }
}
