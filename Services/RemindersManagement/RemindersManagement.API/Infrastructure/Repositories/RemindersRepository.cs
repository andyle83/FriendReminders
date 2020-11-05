using Microsoft.EntityFrameworkCore;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Domain.Models;
using RemindersManagement.API.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemindersManagement.API.Infrastructure.Repositories
{
    public class RemindersRepository : BaseRepository, IRemindersRepository
    {
        public RemindersRepository(RemindersDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Reminder>> ListAsync()
        {
            return await _context.Reminders.AsNoTracking().ToListAsync();
        }

        public async Task<Reminder> FindAsync(Guid id)
        {
            var reminder = await _context.Reminders
                .FirstOrDefaultAsync(r => r.Id == id);

            return reminder;
        }

        public async Task<Reminder> AddAsync(Reminder reminder)
        {
            await _context.Reminders.AddAsync(reminder);
            return reminder;
        }

        public void Update(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
        }

        public void Remove(Reminder reminder)
        {
            _context.Reminders.Remove(reminder);
        }
    }
}