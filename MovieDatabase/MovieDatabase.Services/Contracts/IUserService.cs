using System.Threading.Tasks;

namespace MovieDatabase.Services.Contracts
{
    public interface IUserService
    {
        Task<string> GetUserIdFromUserNameAsync(string userName);
    }
}
