using System;

namespace WebApi.Features.Reminders.DataContracts
{
    public class ReminderResponse
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime? RemiderTime { get; set; }
    }
}