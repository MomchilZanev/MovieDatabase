using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MovieDatabase.Tests
{
    public class AvatarServiceTests
    {
        [Fact]
        public async Task GetUserAvatarLinkShouldReturnDefaultImageIfIdIsInvalid()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserAvatarLink_Db_1")
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

            var avatarService = new AvatarService(dbContext);

            var actualResult = await avatarService.GetUserAvatarLink("invalidId");

            Assert.True(actualResult == GlobalConstants.avatarLinkPrefix + "no_avatar.jpg");
        }

        [Fact]
        public async Task GetUserAvatarLinkShouldReturnImageProperly()
        {
            var options = new DbContextOptionsBuilder<MovieDatabaseDbContext>()
                    .UseInMemoryDatabase(databaseName: "GetUserAvatarLink_Db_2")
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

            var id = dbContext.Users.First().Id;

            var avatarService = new AvatarService(dbContext);

            var actualResult = await avatarService.GetUserAvatarLink(id);

            Assert.True(actualResult == "avatar");
        }
    }
}
