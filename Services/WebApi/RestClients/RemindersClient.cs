using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.RestClients
{
    public interface IReminderClient
    {
        public Task<string> GetReminders(string accessToken, string query);
    }

    public class RemindersClient : IReminderClient
    {
        private readonly ILogger<RemindersClient> _logger;

        private readonly IHttpClientFactory _clientFactory;

        private IConfiguration _configuration;

        public RemindersClient(IConfiguration configurationt, IHttpClientFactory clientFactory, ILogger<RemindersClient> logger)
        {
            _configuration = configurationt;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<string> GetReminders(string accessToken, string query)
        {
            var apiClient = _clientFactory.CreateClient();
            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync($"{_configuration.GetValue<string>("RemindersMgtService:Uri")}/{query}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(response.StatusCode.ToString());
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}