using UserService.ViewModels;

namespace UserService.Interfaces
{
    public interface IToken
    {
        Task<string> LoginUser(LoginModel loginModel);
    }
}
