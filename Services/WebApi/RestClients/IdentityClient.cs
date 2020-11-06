using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.RestClients
{
    public interface IIdentityClient
    {
        public Task<TokenResponse> GetAccessToken();
    }

    public class IdentityClient : IIdentityClient
    {
        private readonly ILogger<IdentityClient> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private IConfiguration _configuration;

        public IdentityClient(IConfiguration configurationt, IHttpClientFactory clientFactory, ILogger<IdentityClient> logger)
        {
            _configuration = configurationt;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<TokenResponse> GetAccessToken()
        {
            // Checking discovery token via HttpClient
            var client = _clientFactory.CreateClient();
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

            return tokenResponse;
        }
    }
}