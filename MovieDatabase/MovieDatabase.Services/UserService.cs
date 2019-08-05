using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.User;
using MovieDatabase.Services.Contracts;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly UserManager<MovieDatabaseUser> userManager;

        public UserService(MovieDatabaseDbContext dbContext, UserManager<MovieDatabaseUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
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

        public async Task<bool> PromoteUserAsync(PromoteUserInputModel input)
        {
            if (!await dbContext.Users.AnyAsync(user => user.UserName == input.Name))
            {
                return false;
            }

            var userFromDb = await dbContext.Users.SingleOrDefaultAsync(user => user.UserName == input.Name);

            if (await userManager.IsInRoleAsync(userFromDb, GlobalConstants.adminRoleName))
            {
                return false;
            }
            
            await userManager.RemoveFromRoleAsync(userFromDb, GlobalConstants.userRoleName);
            await userManager.AddToRoleAsync(userFromDb, GlobalConstants.adminRoleName);

            return true;
        }
    }
}
