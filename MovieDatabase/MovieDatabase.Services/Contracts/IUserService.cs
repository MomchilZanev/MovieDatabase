using MovieDatabase.Models.InputModels.User;
using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IUserService
    {
        Task<string> GetUserIdFromUserNameAsync(string userName);

        Task<bool> PromoteUserAsync(PromoteUserInputModel input);
    }
}
