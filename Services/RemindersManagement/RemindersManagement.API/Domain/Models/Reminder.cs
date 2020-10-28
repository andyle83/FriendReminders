    using System;

namespace RemindersManagement.API.Domain.Models
{
    public enum ReminderStatus
    {
        Doing,
        Finished,
    }

    public enum Priority
    {
        Low,
        Medium,
        Hight
    }

    public class Reminder
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public ReminderStatus Status { get; set; }

        public Priority Priority { get; set; } = Priority.Medium;

        public DateTime? RemiderTime { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public override bool Equals(object obj)
        {
            Reminder reminder = obj as Reminder;

            if (reminder == null)
            {
                return false;
            }

            return (this.Id == reminder.Id)
                && (this.Description == reminder.Description)
                && (this.Status == reminder.Status)
                && (this.Priority == reminder.Priority)
                && (this.RemiderTime == reminder.RemiderTime)
                && (this.CategoryId == reminder.CategoryId)
                && (this.Category.Equals(reminder.Category));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
