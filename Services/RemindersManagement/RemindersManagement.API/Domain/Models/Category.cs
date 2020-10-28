using System;
using System.Collections.Generic;

namespace RemindersManagement.API.Domain.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public string HexaColor { get; set; }

        public List<Reminder> Reminders { get; set; }

        public override bool Equals(object obj)
        {
            Category category = obj as Category;

            if (category == null)
            {
                return false;
            }

            return (this.Id == category.Id)
                && (this.Name == category.Name)
                && (this.Icon == category.Icon)
                && (this.HexaColor == category.HexaColor)
                && (this.Reminders == category.Reminders);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
