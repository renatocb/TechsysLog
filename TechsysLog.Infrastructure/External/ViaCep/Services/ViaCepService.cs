using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TechsysLog.Infrastructure.External.ViaCep.Interfaces;
using TechsysLog.Infrastructure.External.ViaCep.Models;

namespace TechsysLog.Infrastructure.External.ViaCep.Services
{
    public class ViaCepService : IViaCepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ViaCepResponse> GetAddressByCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var address = JsonSerializer.Deserialize<ViaCepResponse>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return address;
        }
    }
}