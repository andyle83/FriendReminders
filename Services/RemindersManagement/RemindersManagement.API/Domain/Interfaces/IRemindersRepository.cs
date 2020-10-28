using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RemindersManagement.API.Domain.Models;

namespace RemindersManagement.API.Domain.Interfaces
{
    public interface IRemindersRepository
    {
        public Task<IEnumerable<Reminder>> ListAsync();
        public Task<Reminder> FindAsync(Guid id);
        public Task<Reminder> AddAsync(Reminder reminder);
        public void Update(Reminder reminder);
        public void Remove(Reminder reminder);
    }
}
