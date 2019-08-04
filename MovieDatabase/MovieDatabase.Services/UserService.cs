using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieDatabase.Common;
using MovieDatabase.Data;
using MovieDatabase.Domain;
using MovieDatabase.Models.InputModels.User;
using MovieDatabase.Models.ViewModels.User;
using MovieDatabase.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieDatabase.Services
{
    public class UserService : IUserService
    {
        private readonly MovieDatabaseDbContext dbContext;
        private readonly UserManager<MovieDatabaseUser> userManager;
        private readonly IMapper mapper;

        public UserService(MovieDatabaseDbContext dbContext, UserManager<MovieDatabaseUser> userManager, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.mapper = mapper;
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

        public async Task<List<UserNameViewModel>> GetAllRegularUserNamesAsync()
        {
            var allRegularUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.userRoleName);

            var allRegularUserNamesViewModel = mapper.Map<IList<MovieDatabaseUser>, List<UserNameViewModel>>(allRegularUsers);

            return allRegularUserNamesViewModel;
        }

        public async Task<bool> PromoteUserAsync(PromoteUserInputModel input)
        {
            if (!await dbContext.Users.AnyAsync(user => user.UserName == input.Name))
            {
                return false;
            }

            var regularUsers = await userManager.GetUsersInRoleAsync(GlobalConstants.userRoleName);
            if (!regularUsers.Any(user => user.UserName == input.Name))
            {
                return false;
            }

            var userFromDb = await dbContext.Users.SingleOrDefaultAsync(user => user.UserName == input.Name);
            await userManager.RemoveFromRoleAsync(userFromDb, GlobalConstants.userRoleName);
            await userManager.AddToRoleAsync(userFromDb, GlobalConstants.adminRoleName);

            return true;
        }
    }
}
