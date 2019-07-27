using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
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

            var userService = new UserService(dbContext);

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

            var userService = new UserService(dbContext);

            var actualId = await userService.GetUserIdFromUserNameAsync("user1");

            Assert.Equal(actualId, expectedId);
        }
    }
}
