using MovieDatabase.Models.InputModels.User;
using MovieDatabase.Models.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IUserService
    {
        Task<string> GetUserIdFromUserNameAsync(string userName);

        Task<List<UserNameViewModel>> GetAllRegularUserNamesAsync();

        Task<bool> PromoteUserAsync(PromoteUserInputModel input);
    }
}
