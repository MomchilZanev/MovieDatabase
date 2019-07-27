using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Services.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class AvatarService : IAvatarService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public AvatarService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task ChangeUserAvatarAsync(string userId, IFormFile avatar)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (avatar != null && avatar.Length > 0 && avatar.Length < ValidationConstants.userAvatarMaximumFileSizeInBytes)
            {
                var fileName = user.UserName + GlobalConstants.imageFileSuffix;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.userAvatarsDirectory, fileName);

                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileSrteam);
                }

                user.AvatarLink = GlobalConstants.avatarLinkPrefix + fileName;

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetUserAvatarLink(string userId)
        {
            if (await dbContext.Users.AnyAsync(user => user.Id == userId))
            {
                var userFromDb = await dbContext.Users.FindAsync(userId);
                return userFromDb.AvatarLink;
            }

            return GlobalConstants.avatarLinkPrefix + "no_avatar.jpg";
        }
    }
}
