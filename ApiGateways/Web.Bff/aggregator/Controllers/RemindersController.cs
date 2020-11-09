using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.RestClients;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemindersController : ControllerBase
    {
        private readonly ILogger<RemindersController> _logger;
        private IIdentityClient _identityClient;
        private IReminderClient _reminderClient;

        public RemindersController(ILogger<RemindersController> logger, IIdentityClient identityClient, IReminderClient reminderClient)
        {
            _identityClient = identityClient;
            _reminderClient = reminderClient;
            _logger = logger;
        }

        [HttpGet]
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