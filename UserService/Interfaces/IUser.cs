using System.Threading.Tasks;
using UserService.Models;
using UserService.ViewModels;

namespace UserService.Interfaces
{
    public interface IUser
    {
        Task<long> AddUser(CreateUserModel request);
        Task<long> UpdateUser(long id,UpdateUserModel request);
        Task<ViewUserModel> GetUserById(long id);
        Task<List<ViewUserModel>> GetUserList(FilterRequest<UserRequest> request);
    }
}
