using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
using MovieDatabase.Web.AutoMapperProfiles;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUserIdFromUsrnameShouldReturnNullIfInputIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserIdFromUsrname_Db_1")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var mockUserManager = new Mock<UserManager<MovieDatabaseUser>>(
                    new Mock<IUserStore<MovieDatabaseUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<MovieDatabaseUser>>().Object,
                    new IUserValidator<MovieDatabaseUser>[0],
                    new IPasswordValidator<MovieDatabaseUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<MovieDatabaseUser>>>().Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UsersProfile());
            });
            var mapper = config.CreateMapper();
            var userService = new UserService(dbContext, mockUserManager.Object, mapper);

            var actualResult = await userService.GetUserIdFromUserNameAsync("invalid");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetUserIdFromUsrnameShouldReturnIdProperlyIfdataIsValid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserIdFromUsrname_Db_2")
                    .Options;
            var dbContext = new MovieDatabaseDbContext(options);

            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var expectedId = dbContext.Users.First().Id;

            var mockUserManager = new Mock<UserManager<MovieDatabaseUser>>(
                    new Mock<IUserStore<MovieDatabaseUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<MovieDatabaseUser>>().Object,
                    new IUserValidator<MovieDatabaseUser>[0],
                    new IPasswordValidator<MovieDatabaseUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<MovieDatabaseUser>>>().Object);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UsersProfile());
            });
            var mapper = config.CreateMapper();
            var userService = new UserService(dbContext, mockUserManager.Object, mapper);

            var actualId = await userService.GetUserIdFromUserNameAsync("user1");

            Assert.Equal(actualId, expectedId);
        }
    }
}
