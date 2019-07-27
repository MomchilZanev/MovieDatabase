using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public UserService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<string> GetUserIdFromUserNameAsync(string userName)
        {
            if (await dbContext.Users.AnyAsync(user => user.UserName == userName))
            {
                var userFromDb = await dbContext.Users.SingleOrDefaultAsync(user => user.UserName == userName);

                return userFromDb.Id;
            }

            return null;
        }
    }
}
