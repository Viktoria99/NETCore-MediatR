using System.Threading.Tasks;

namespace Ddd.Example.Service.Domain.Users.V10
{

    public interface IUserRepository
    {

        Task<User> GetUserAsync(int userId);

        Task<User> FindUserAsync(string name);
    }
}
