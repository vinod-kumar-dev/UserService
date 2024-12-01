using System.Threading.Tasks;
using UserAPI.Models;
using UserAPI.ViewModels;

namespace UserAPI.Interfaces
{
    public interface IUser
    {
        Task<long> AddUser(CreateUserModel request);
        Task<long> UpdateUser(long id,UpdateUserModel request);
        Task<ViewUserModel> GetUserById(long id);
        Task<List<ViewUserModel>> GetUserList(FilterRequest<UserRequest> request);
    }
}
