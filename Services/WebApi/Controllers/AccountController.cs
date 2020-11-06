using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.RestClients;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private IIdentityClient _identityClient;

        public AccountController(ILogger<AccountController> logger, IIdentityClient identityClient)
        {
            _identityClient = identityClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Checking discovery token via HttpClient
            _logger.LogInformation("Get access token from a REST Client");
            var token = await _identityClient.GetAccessToken();

            return Ok(token);
        }
    }
}