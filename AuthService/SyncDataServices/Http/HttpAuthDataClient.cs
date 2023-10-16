using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AuthService.Models;

namespace AuthService.SyncDataServices.Http
{
    public class HttpAuthDataClient : IAuthDataClient
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;
        public HttpAuthDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendCustomerToAuth(CustomerReadModel customer)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(customer),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["AuthService"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
            }
        }
    }
}