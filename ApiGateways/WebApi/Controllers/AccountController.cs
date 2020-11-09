using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.RestClients;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private IIdentityClient _identityClient;
        private IReminderClient _reminderClient;

        public AccountController(ILogger<AccountController> logger, IIdentityClient identityClient, IReminderClient reminderClient)
        {
            _identityClient = identityClient;
            _reminderClient = reminderClient;
            _logger = logger;
        }

        [HttpGet]
        [Route("Token/")]
        public async Task<IActionResult> GetToken()
        {
            // Checking discovery token via HttpClient
            _logger.LogInformation("Get access token from a REST Client");
            var token = await _identityClient.GetAccessToken();

            return Ok(token);
        }

        [HttpGet]
        [Route("Reminders/")]
        public async Task<IActionResult> GetReminders()
        {
            _logger.LogInformation("Get access token from REST Client of Identity Server");
            var tokenResponse = await _identityClient.GetAccessToken();

            _logger.LogInformation("Get reminders from REST Client of Reminders Management Service");
            var result = await _reminderClient.GetReminders(tokenResponse.AccessToken, "api/Reminders");

            return Ok(result);
        }
    }
}