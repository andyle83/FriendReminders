using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RemindersManagement.API.Domain.Exceptions;
using RemindersManagement.API.Domain.Interfaces;
using RemindersManagement.API.Domain.Models;
using RemindersManagement.API.Domain.Services;
using Xunit;

namespace RemindersManagement.UnitTests.Application.Services
{
    public class RemindersServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemindersService>> _loggerMock;

        public RemindersServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemindersService>>();
        }

        [Fact]
        public async Task Get_Reminders_Async_Success()
        {
            // Arrange
            var reminders = new List<Reminder>()
            {
                new Reminder()
                {
                    Id = Guid.NewGuid(),
                    Description = "Working",
                    Status = ReminderStatus.Doing
                },
                new Reminder()
                {
                    Id = Guid.NewGuid(),
                    Description = "Vanishing",
                    Status = ReminderStatus.Finished
                }
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.ListAsync())
                .Returns(Task.FromResult<IEnumerable<Reminder>>(reminders));

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            var listResult = await remindersService.GetRemindersAsync();

            // Assert
            Assert.NotNull(listResult);
            Assert.Equal((listResult as List<Reminder>).Count, reminders.Count);
        }

        [Fact]
        public async Task Get_A_Reminder_Async_Success()
        {
            // Arrange
            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.FindAsync(reminder.Id))
                .Returns(Task.FromResult(reminder));

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            var reminderResult = await remindersService.GetReminderAsync(reminder.Id);

            // Assert
            Assert.NotNull(reminderResult);
            Assert.IsType<Reminder>(reminderResult);
            Assert.Equal(reminderResult, reminder);
        }

        [Fact]
        public async Task Put_A_Reminder_Async_Success()
        {
            // Arrange
            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.AddAsync(reminder))
                .Returns(Task.FromResult(reminder));

            _unitOfWorkMock.Setup(x => x.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            var reminderResult = await remindersService.PutReminderAsync(reminder);

            // Arrange
            Assert.NotNull(reminderResult);
            Assert.Equal(reminderResult, reminder);
        }


        [Fact]
        public async Task Put_A_Reminder_Async_Failed()
        {
            // Arrange
            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.AddAsync(reminder))
                .Throws(new Exception());

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);

            // Assert
            var createResult = await Assert.ThrowsAsync<Exception>(() => remindersService.PutReminderAsync(reminder));
            Assert.NotNull(createResult);
        }

        [Fact]
        public async Task Update_A_Reminder_Success()
        {
            // Arrange
            var reminderId = Guid.NewGuid();
            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.FindAsync(reminderId))
                .Returns(Task.FromResult(reminder));

            _unitOfWorkMock.Setup(x => x.RemindersRepository.Update(reminder));

            _unitOfWorkMock.Setup(x => x.CommitAsync())
                .Returns(Task.CompletedTask);

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            var updateResult = await remindersService.PutReminderAsync(reminderId, reminder);

            Assert.NotNull(updateResult);
            Assert.Equal(updateResult, reminder);
        }

        [Fact]
        public async Task Update_A_Reminder_Failed_With_Not_Found_Exception()
        {
            // Arrange
            var reminderId = Guid.NewGuid();
            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.FindAsync(reminderId))
                .Returns(Task.FromResult((Reminder)null));


            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            var updateResult = await Assert.ThrowsAsync<ReminderNotFoundException>(() => remindersService.PutReminderAsync(reminderId, reminder));

            Assert.NotNull(updateResult);
        }

        [Fact]
        public async Task Update_A_Reminder_Failed_With_An_Exception()
        {
            // Arrange
            var reminderId = Guid.NewGuid();
            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _unitOfWorkMock.Setup(x => x.RemindersRepository.FindAsync(reminderId))
                .Returns(Task.FromResult(reminder));

            _unitOfWorkMock.Setup(x => x.RemindersRepository.Update(reminder))
                .Throws(new Exception());

            // Act
            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);

            var updateResult = await Assert.ThrowsAsync<Exception>(() => remindersService.PutReminderAsync(reminderId, reminder));
            Assert.NotNull(updateResult);
        }

        [Fact]
        public async Task Delete_A_Reminder_Failed_With_Not_Found_Exception()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            _unitOfWorkMock.Setup(x => x.RemindersRepository.FindAsync(reminderId))
                .Returns(Task.FromResult((Reminder)null));

            var remindersService = new RemindersService(_unitOfWorkMock.Object, _loggerMock.Object);
            await Assert.ThrowsAsync<ReminderNotFoundException>(() => remindersService.DeleteReminder(reminderId));
        }
    }
}
