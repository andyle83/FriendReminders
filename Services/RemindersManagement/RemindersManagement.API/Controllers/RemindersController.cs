using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RemindersManagement.API.Domain.Models;
using RemindersManagement.API.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindersManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly ILogger<RemindersController> _logger;
        private readonly IRemindersService _remindersService;

        public RemindersController(ILogger<RemindersController> logger, IRemindersService remindersService)
        {
            _logger = logger;
            _remindersService = remindersService;
        }

        // GET: api/reminders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reminder>>> GetReminders()
        {
            var reminders = await _remindersService.GetRemindersAsync();
            return Ok(reminders);
        }

        // GET: api/reminder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reminder>> GetReminder(Guid id)
        {
            var reminder = await _remindersService.GetReminderAsync(id);

            if (reminder == null)
            {
                _logger.LogWarning($"Could not found {id}.", id);
                return NotFound();
            }

            return reminder;
        }

        // PUT: api/reminders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReminder(Guid id, Reminder reminder)
        {
            if (id != reminder.Id)
            {
                return BadRequest();
            }

            try
            {
                await _remindersService.PutReminderAsync(id, reminder);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error when update reminder", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        // POST: api/reminders
        [HttpPost]
        public async Task<ActionResult<Reminder>> PostReminder(Reminder reminder)
        {
            if (reminder.CategoryId == Guid.Empty)
            {
                return BadRequest();
            }

            await _remindersService.PutReminderAsync(reminder);

            return CreatedAtAction("GetReminder", new { id = reminder.Id }, reminder);
        }

        // DELETE: api/reminders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Reminder>> DeleteReminder(Guid id)
        {
            var reminder = await _remindersService.GetReminderAsync(id);

            if (reminder == null)
            {
                _logger.LogWarning($"Could not found {id} to delete.", id);
                return NotFound();
            }

            try
            {
                await _remindersService.DeleteReminder(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when delete reminder {ex}", ex.Message);
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return reminder;
        }
    }
}