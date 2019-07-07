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
            var user = dbContext.Users.Find(userId);

            if (avatar != null && avatar.Length > 0 && avatar.Length < 64000)
            {
                var fileName = user.UserName + ".jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\user_avatars", fileName);
                using (var fileSrteam = new FileStream(filePath, FileMode.Create))
                {
                    await avatar.CopyToAsync(fileSrteam);
                }
                user.AvatarLink = $"/user_avatars/{fileName}";

                dbContext.Update(user);
                dbContext.SaveChanges();
            }
        }

        public string GetUserAvatarLink(string userId)
        {
            var user = dbContext.Users.Find(userId);

            return user.AvatarLink;
        }
    }
}
