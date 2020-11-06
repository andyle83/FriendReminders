using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Checking discovery token via HttpClient
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

            if (disco.IsError)
            {
                _logger.LogError(disco.Error);
                return BadRequest();
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
                return BadRequest();
            }

            return Ok(tokenResponse.Json);
        }
    }
}