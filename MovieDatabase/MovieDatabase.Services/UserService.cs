using MovieDatabase.Data;
using MovieDatabase.Services.Contracts;
using System.Linq;

namespace MovieDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly MovieDatabaseDbContext dbContext;

        public UserService(MovieDatabaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GetUserIdFromUserName(string userName)
        {
            if (dbContext.Users.Any(user => user.UserName == userName))
            {
                var userId = dbContext.Users.SingleOrDefault(user => user.UserName == userName).Id;

                return userId;
            }

            return null;
        }
    }
}
