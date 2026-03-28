using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FortivexAPI.Controllers;
using FortivexAPI.Data;
using FortivexAPI.Dtos;
using FortivexAPI.Helpers;
using FortivexAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FortivexAPI.Tests
{
    public class AccountsControllerTests
    {
        private FortivexContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<FortivexContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new FortivexContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private JwtHelper GetJwtHelper()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:Key", "EzEgyNagyonHosszuEsTitkosKulcsATesztekhez123!"},
                {"Jwt:Issuer", "FortivexTest"},
                {"Jwt:Audience", "FortivexUsers"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            return new JwtHelper(configuration);
        }

        [Fact]
        public async Task GetAccounts_ReturnsOkResult()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AccountsController(context, GetJwtHelper());

            // Act
            var result = await controller.GetAccounts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var accounts = Assert.IsAssignableFrom<IEnumerable<AccountDto>>(okResult.Value);
            Assert.NotEmpty(accounts); // A Context Seed adatai miatt nem lehet üres
        }

        [Fact]
        public async Task GetAccount_ReturnsNotFound_WhenIdInvalid()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AccountsController(context, GetJwtHelper());

            // Act
            var result = await controller.GetAccount(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Register_CreatesNewUser_AndReturnsToken()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AccountsController(context, GetJwtHelper());
            var registerDto = new RegisterRequestDto
            {
                UserName = "newtestuser",
                PasswordHash = "password123",
                Email = "test@example.com",
                Role = "user"
            };

            // Act
            var result = await controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            var userInDb = await context.Accounts.AnyAsync(a => a.Username == "newtestuser");
            Assert.True(userInDb);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithValidCredentials()
        {
            // Arrange
            using var context = GetInMemoryContext();
            // A Seed adatok között szerepel a 'testplayer' / 'test123' páros
            var controller = new AccountsController(context, GetJwtHelper());
            var loginDto = new LoginRequestDto { UserName = "testplayer", PasswordHash = "test123" };

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task DeleteAccount_RemovesAccountFromDb()
        {
            // Arrange
            using var context = GetInMemoryContext();
            var controller = new AccountsController(context, GetJwtHelper());
            // A Seed adatokból az 1-es ID létezik

            // Act
            var result = await controller.DeleteAccount(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var accountExists = await context.Accounts.AnyAsync(a => a.Id == 1);
            Assert.False(accountExists);
        }
    }
}