using System;
namespace RemindersManagement.API.Domain.Exceptions
{
    public class ReminderNotFoundException : Exception
    {
        public ReminderNotFoundException()
        {
        }

        public ReminderNotFoundException(string message)
            : base(message)
        {
        }

        public ReminderNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
