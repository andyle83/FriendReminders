using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RemindersManagement.API.Domain.Exceptions;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Domain.Models;

namespace RemindersManagement.API.Domain.Services
{
    public interface IRemindersService
    {
        Task<IEnumerable<Reminder>> GetRemindersAsync();
        Task<Reminder> GetReminderAsync(Guid id);
        Task<Reminder> PutReminderAsync(Reminder reminder);
        Task<Reminder> DeleteReminder(Guid id);
        Task<Reminder> PutReminderAsync(Guid id, Reminder reminder);
    }

    public class RemindersService : IRemindersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemindersService> _logger;

        public RemindersService(IUnitOfWork unitOfWork, ILogger<RemindersService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Reminder>> GetRemindersAsync()
        {
            return await _unitOfWork.RemindersRepository.ListAsync();
        }

        public async Task<Reminder> GetReminderAsync(Guid id)
        {
            return await _unitOfWork.RemindersRepository.FindAsync(id);
        }

        public async Task<Reminder> PutReminderAsync(Reminder reminder)
        {
            try
            {
                var result = await _unitOfWork.RemindersRepository.AddAsync(reminder);
                await _unitOfWork.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when create reminder {ex}", ex.Message);
                throw ex;
            }
        }

        public async Task<Reminder> PutReminderAsync(Guid id, Reminder reminder)
        {
            var existingReminder = await _unitOfWork.RemindersRepository.FindAsync(id);

            if (existingReminder == null)
            {
                throw new ReminderNotFoundException($"Reminder {id} is not found.");
            }

            existingReminder.Description = reminder.Description;

            try
            {
                _unitOfWork.RemindersRepository.Update(existingReminder);
                await _unitOfWork.CommitAsync();

                return existingReminder;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when update reminder {ex}", ex.Message);
                throw ex;
            }
        }

        public async Task<Reminder> DeleteReminder(Guid id)
        {
            var existingReminder = await _unitOfWork.RemindersRepository.FindAsync(id);

            if (existingReminder == null)
            {
                throw new ReminderNotFoundException($"Reminder {id} is not found.");
            }

            try
            {
                _unitOfWork.RemindersRepository.Remove(existingReminder);
                await _unitOfWork.CommitAsync();

                return existingReminder;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when delete reminder {ex}", ex.Message);
                throw ex;
            }
        }
    }
}
