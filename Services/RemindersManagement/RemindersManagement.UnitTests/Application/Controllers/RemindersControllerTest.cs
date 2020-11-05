using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RemindersManagement.API.Controllers;
using RemindersManagement.API.Domain.Exceptions;
using RemindersManagement.API.Domain.Models;
using RemindersManagement.API.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RemindersManagement.UnitTests.Application.Controllers
{
    public class RemindersControllerTest
    {
        private readonly Mock<IRemindersService> _remindersServiceMock;
        private readonly Mock<ILogger<RemindersController>> _loggerMock;

        public RemindersControllerTest()
        {
            _remindersServiceMock = new Mock<IRemindersService>();
            _loggerMock = new Mock<ILogger<RemindersController>>();
        }

        [Fact]
        public async Task List_Reminders_Success()
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

            _remindersServiceMock
                .Setup(x => x.GetRemindersAsync())
                .Returns(Task.FromResult<IEnumerable<Reminder>>(reminders));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var listResult = await remindersController.GetReminders();

            // Assert
            Assert.NotNull(listResult);
            Assert.Equal((listResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((((OkObjectResult)listResult.Result).Value as List<Reminder>).Capacity, reminders.Capacity);
        }

        [Fact]
        public async Task Get_Reminder_Success()
        {
            // Arrange
            var remindersList = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Default",
                HexaColor = "#fff",
                Icon = "Default",
            };

            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing,
                Priority = Priority.Hight,
                RemiderTime = DateTime.Now,
                CategoryId = remindersList.Id,
                Category = remindersList
            };

            remindersList.Reminders = new List<Reminder> { reminder };

            _remindersServiceMock
                .Setup(x => x.GetReminderAsync(reminder.Id))
                .Returns(Task.FromResult(reminder));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var reminderResult = await remindersController.GetReminder(reminder.Id);

            // Assert
            Assert.NotNull(reminderResult);
            // Assert.Equal<Reminder>(remindersResult.Value as Reminder, reminder1);
            // Assert by properties
            Assert.True((reminderResult.Value as Reminder).Equals(reminder));
        }

        [Fact]
        public async Task Get_Reminder_NotFound()
        {
            // Arrange
            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.GetReminderAsync(reminder.Id))
                .Returns(Task.FromResult((Reminder)null));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var getResult = await remindersController.GetReminder(reminder.Id);

            // Assert
            Assert.Equal((getResult.Result as NotFoundResult).StatusCode, (int)System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_Reminder_Success()
        {
            // Arrange
            var reminder = new Reminder()
            {
                Description = "Working",
                Status = ReminderStatus.Doing,
                CategoryId = Guid.NewGuid()
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminder))
                .Returns(Task.FromResult(reminder));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var createResult = await remindersController.PostReminder(reminder);

            // Assert
            Assert.NotNull(createResult);
            Assert.IsType<CreatedAtActionResult>(createResult.Result);
        }

        [Fact]
        public async Task Create_Reminder_Failed()
        {
            // Arrange
            // Description is Null
            var reminder = new Reminder()
            {
                Status = ReminderStatus.Doing,
                CategoryId = Guid.NewGuid()
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminder))
                .Throws(new Exception());

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);

            // Assert
            var createResult = await Assert.ThrowsAsync<Exception>(() => remindersController.PostReminder(reminder));
            Assert.NotNull(createResult);
        }

        [Fact]
        public async Task Update_Reminder_Success()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminderId, reminder))
                .Returns(Task.FromResult((Reminder)null));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var udpateResult = await remindersController.PutReminder(reminderId, reminder);

            // Assert
            Assert.NotNull(udpateResult);
            Assert.Equal((udpateResult as NoContentResult).StatusCode, (int)System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Update_Reminder_Failed_With_Bad_Request()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = Guid.NewGuid(),
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminderId, reminder))
                .Returns(Task.FromResult((Reminder)null));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var udpateResult = await remindersController.PutReminder(reminderId, reminder);

            // Assert
            Assert.NotNull(udpateResult);
            Assert.Equal((udpateResult as BadRequestResult).StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_Reminder_Failed_With_Server_Error()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminderId, reminder))
                .Throws(new ReminderNotFoundException());

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var udpateResult = await remindersController.PutReminder(reminderId, reminder);

            // Assert
            Assert.NotNull(udpateResult);
            Assert.Equal((udpateResult as ObjectResult).StatusCode, (int)System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Update_Reminder_Failed_With_Reminder_Not_Found_Exception()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.PutReminderAsync(reminderId, reminder))
                .Throws(new ReminderNotFoundException($"Reminder {reminderId} is not found."));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var udpateResult = await remindersController.PutReminder(reminderId, reminder);

            // Assert
            Assert.NotNull(udpateResult);
            Assert.Equal((udpateResult as ObjectResult).StatusCode, (int)System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Delete_Reminder_Success()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.GetReminderAsync(reminderId))
                .Returns(Task.FromResult(reminder));

            _remindersServiceMock
                .Setup(x => x.DeleteReminder(reminderId))
                .Returns(Task.FromResult(reminder));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var deleteResult = await remindersController.DeleteReminder(reminderId);

            // Assert
            Assert.NotNull(deleteResult);
            Assert.NotNull(deleteResult.Value);
        }

        [Fact]
        public async Task Delete_Reminder_Not_Found_Result()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.GetReminderAsync(reminderId))
                .Returns(Task.FromResult((Reminder)null));

            _remindersServiceMock
                .Setup(x => x.DeleteReminder(reminderId))
                .Returns(Task.FromResult(reminder));

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var deleteResult = await remindersController.DeleteReminder(reminderId);

            // Assert
            Assert.NotNull(deleteResult);
            Assert.Equal((deleteResult.Result as NotFoundResult).StatusCode, (int)System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Reminder_Failed_With_Server_Error()
        {
            // Arrange
            var reminderId = Guid.NewGuid();

            var reminder = new Reminder()
            {
                Id = reminderId,
                Description = "Working",
                Status = ReminderStatus.Doing
            };

            _remindersServiceMock
                .Setup(x => x.GetReminderAsync(reminderId))
                .Returns(Task.FromResult(reminder));

            _remindersServiceMock
                .Setup(x => x.DeleteReminder(reminderId))
                .Throws(new Exception());

            // Act
            var remindersController = new RemindersController(_loggerMock.Object, _remindersServiceMock.Object);
            var deleteResult = await remindersController.DeleteReminder(reminderId);

            // Assert
            Assert.NotNull(deleteResult);
            Assert.Equal((deleteResult.Result as ObjectResult).StatusCode, (int)System.Net.HttpStatusCode.InternalServerError);
        }
    }
}