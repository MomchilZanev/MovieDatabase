using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class AvatarServiceTests
    {
        private readonly MovieDatabaseDbContext dbContext;

        public AvatarServiceTests()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            this.dbContext = new MovieDatabaseDbContext(options);
        }

        [Fact]
        public async Task GetUserAvatarLinkShouldReturnDefaultImageIfIdIsInvalid()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var avatarService = new AvatarService(dbContext);

            var actualResult = await avatarService.GetUserAvatarLink("invalidId");

            Assert.True(actualResult == GlobalConstants.avatarLinkPrefix + "no_avatar.jpg");
        }

        [Fact]
        public async Task GetUserAvatarLinkShouldReturnImageProperly()
        {
            var user = new MovieDatabaseUser
            {
                AvatarLink = "avatar",
                Email = "email@mail.com",
                UserName = "user1",
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var id = dbContext.Users.First().Id;

            var avatarService = new AvatarService(dbContext);

            var actualResult = await avatarService.GetUserAvatarLink(id);

            Assert.True(actualResult == "avatar");
        }
    }
}
