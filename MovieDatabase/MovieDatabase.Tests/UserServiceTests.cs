using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.User;
using MovieDatabase.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class UserServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly Mock<UserManager<MovieDatabaseUser>> mockUserManager;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);

            this.mockUserManager = new Mock<UserManager<MovieDatabaseUser>>(
                    new Mock<IUserStore<MovieDatabaseUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<MovieDatabaseUser>>().Object,
                    new IUserValidator<MovieDatabaseUser>[0],
                    new IPasswordValidator<MovieDatabaseUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<MovieDatabaseUser>>>().Object);
        }

        [Fact]
        public async Task GetUserIdFromUsrnameShouldReturnNullIfInputIsInvalid()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var userService = new UserService(dbContext, mockUserManager.Object);

            var actualResult = await userService.GetUserIdFromUserNameAsync("invalid");

            Assert.True(actualResult == null);
        }

        [Fact]
        public async Task GetUserIdFromUsrnameShouldReturnIdProperlyIfdataIsValid()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var expectedId = dbContext.Users.First().Id;

            var userService = new UserService(dbContext, mockUserManager.Object);

            var actualId = await userService.GetUserIdFromUserNameAsync("user1");

            Assert.Equal(actualId, expectedId);
        }

        [Fact]
        public async Task PromoteUserShouldReturnFalseIfNoSuchUserExists()
        {
            var userService = new UserService(dbContext, mockUserManager.Object);

            var result = await userService.PromoteUserAsync(new PromoteUserInputModel { Name = "Name 1" });

            Assert.False(result);
            Assert.True(dbContext.UserRoles.Count() == 0);
        }

        [Fact]
        public async Task PromoteUserShouldReturnFalseIfUserIsNotInRegularUserRole()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };

            var adminRole = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };

            await dbContext.Users.AddAsync(user);
            await dbContext.Roles.AddAsync(adminRole);
            await dbContext.SaveChangesAsync();

            var adminRoleId = adminRole.Id;

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(true);

            var userService = new UserService(dbContext, mockUserManager.Object);

            var result = await userService.PromoteUserAsync(new PromoteUserInputModel { Name = "user1" });
            Assert.False(result);
        }

        [Fact]
        public async Task PromoteUserShouldPromoteUserToAdmin()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };

            var regularRole = new IdentityRole { Name = "User", NormalizedName = "USER" };
            var adminRole = new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" };

            await dbContext.Users.AddAsync(user);
            await dbContext.Roles.AddAsync(regularRole);
            await dbContext.Roles.AddAsync(adminRole);
            await dbContext.SaveChangesAsync();

            var adminRoleId = adminRole.Id;

            mockUserManager.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(false);

            var userService = new UserService(dbContext, mockUserManager.Object);

            var result = await userService.PromoteUserAsync(new PromoteUserInputModel { Name = "user1" });
            Assert.True(result);
        }
    }
}
