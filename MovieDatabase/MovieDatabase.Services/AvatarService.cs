using Microsoft.AspNetCore.Http;
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

        public async Task ChangeUserAvatar(string userId, IFormFile avatar)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (avatar != null && avatar.Length > 0 && avatar.Length < 64000)
            {
                var fileName = user.UserName + ".jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\user_avatars", fileName);

                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileSrteam);
                }

                user.AvatarLink = $"/user_avatars/{fileName}";

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<string> GetUserAvatarLink(string userId)
        {
            var user = await dbContext.Users.FindAsync(userId);

            return user.AvatarLink;
        }
    }
}
