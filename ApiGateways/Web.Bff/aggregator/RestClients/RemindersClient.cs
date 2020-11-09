using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Features.Reminders.DataContracts;

namespace WebApi.RestClients
{
    public interface IReminderClient
    {
        public Task<List<ReminderResponse>> GetReminders(string accessToken, string query);
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

        public async Task<List<ReminderResponse>> GetReminders(string accessToken, string query)
        {
            var apiClient = _clientFactory.CreateClient();

            apiClient.SetBearerToken(accessToken);

            try
            {
                var response = await Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3))
                    .ExecuteAsync(() => apiClient.GetAsync($"{_configuration.GetValue<string>("RemindersMgtService:Uri")}/{query}"));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(response.StatusCode.ToString());
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<List<ReminderResponse>>(responseContent);

                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Unable to get reminders list via HTTP request {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError($"Unable to deserialize reminders result {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to return reminders list {ex.Message}");
            }

            return null;
        }
    }
}