using Microsoft.VisualStudio.TestTools.UnitTesting;
using FortivexAPI.Controllers;
using FortivexAPI.Data;
using FortivexAPI.Helpers;
using FortivexAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FortivexAPI.Tests.MsTest
{
    [TestClass] // xUnit-nál nincs ilyen, MSTest-nél kötelező
    public class AccountsMsTests
    {
        private FortivexContext GetContext()
        {
            var options = new DbContextOptionsBuilder<FortivexContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new FortivexContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private JwtHelper GetJwt()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> {
                    {"Jwt:Key", "SuperSecretMsTestKey1234567890123456"},
                    {"Jwt:Issuer", "Test"},
                    {"Jwt:Audience", "Test"}
                }).Build();
            return new JwtHelper(config);
        }

        [TestMethod] // xUnit [Fact] megfelelője
        public async Task GetAccounts_ReturnsOk()
        {
            // Arrange
            using var context = GetContext();
            var controller = new AccountsController(context, GetJwt());

            // Act
            var result = await controller.GetAccounts();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }
    }
}