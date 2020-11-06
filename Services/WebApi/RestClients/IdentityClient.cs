using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.RestClients
{
    public interface IIdentityClient
    {
        public Task<JObject> GetAccessToken();
    }

    public class IdentityClient : IIdentityClient
    {
        private readonly ILogger<IdentityClient> _logger;
        private IConfiguration _configuration;

        public IdentityClient(IConfiguration configurationt, ILogger<IdentityClient> logger)
        {
            _configuration = configurationt;
            _logger = logger;
        }

        public async Task<JObject> GetAccessToken()
        {
            // Checking discovery token via HttpClient
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(_configuration.GetValue<string>("IdentityServer:Uri"));

            if (disco.IsError)
            {
                _logger.LogError(disco.Error);
                return null;
            }

            // Request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "webapi",
                ClientSecret = "secret",
                Scope = "remindersmgt"
            });

            if (tokenResponse.IsError)
            {
                _logger.LogError(tokenResponse.Error);
                return null;
            }

            return tokenResponse.Json;
        }
    }
}