﻿using Xunit;
using RemindersManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace RemindersManagement.UnitTests.Application.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void Redirect_To_SwaggerUI()
        {
            // Act
            var homeController = new HomeController();
            var result = homeController.RedirectToSwaggerUI();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectResult>(result);
            Assert.Equal("/swagger/", (result as RedirectResult).Url);
        }
    }
}