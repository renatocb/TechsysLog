using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechsysLog.Web.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Web;

namespace TechsysLog.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            string _apiBaseUrl = ConfigurationManager.AppSettings["apiBaseUrl"];
            int _timeoutSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["timeoutSeconds"]);

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseUrl),
                Timeout = TimeSpan.FromSeconds(_timeoutSeconds) // Adicione um timeout de 30 segundos
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Método para configurar o token no cabeçalho de autorização
        private void SetAuthorizationHeader()
        {
            // Recupera o token do cookie
            string token = HttpContext.Current.Request.Cookies["AuthToken"]?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                // Adiciona o token ao cabeçalho de autorização
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
        public async Task<(bool Success, string ErrorMessage, ResponseUsuarioDto usuario)> RegisterUsuarioAsync(RegistroUsuarioDto usuarioDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(usuarioDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("usuarios", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<ResponseUsuarioDto>(result);
                return (true, null, usuario);
            }
            else
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                return (false, errorResult, null);
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("usuarios/login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<string>(result);
            }
            return null;
        }

        public async Task<UsuarioDto> LoginUsuarioAsync(string email, string senha)
        {
            var loginDto = new LoginDto
            {
                Email = email,
                Senha = senha
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioDto>(result);
            }
            return null;
        }

        public async Task<(bool Success, string ErrorMessage, PedidoDto pedido)> CadastrarPedidoAsync(PedidoDto pedidoDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(pedidoDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("pedidos", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var pedido = JsonConvert.DeserializeObject<PedidoDto>(result);
                return (true, null, pedido);
            }
            else
            {
                var errorResult = await response.Content.ReadAsStringAsync();
                return (false, errorResult, null);
            }
        }

        public async Task<bool> RegistrarEntregaAsync(EntregaDto entregaDto)
        {
            SetAuthorizationHeader();

            var content = new StringContent(JsonConvert.SerializeObject(entregaDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("entregas", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ResponsePedidoDto>> ObterTodosPedidosAsync()
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync("pedidos");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None
                };

                var teste = JsonConvert.DeserializeObject<IEnumerable<ResponsePedidoDto>>(result, settings);

                return JsonConvert.DeserializeObject<IEnumerable<ResponsePedidoDto>>(result, settings);
            }
            return null;
        }

        public async Task<EnderecoDto> ObterEnderecoPorCepAsync(string cep)
        {
            try
            {
                Console.WriteLine($"Calling API with CEP: {cep}");
                var response = await _httpClient.GetAsync($"viacep/{cep}");
                Console.WriteLine($"API call completed with status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<EnderecoDto>(result);
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Deserialization error: {jsonEx.Message}");
                        throw;
                    }
                }
                else
                {
                    // Log the error response
                    var errorResult = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API call failed with status code {response.StatusCode}: {errorResult}");
                    throw new Exception($"API call failed with status code {response.StatusCode}: {errorResult}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while calling the API: {ex.Message}");
                throw new Exception($"An error occurred while calling the API: {ex.Message}", ex);
            }
        }
    }
}

