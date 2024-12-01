using UserAPI.ViewModels;

namespace UserAPI.Interfaces
{
    public interface IToken
    {
        Task<string> LoginUser(LoginModel loginModel);
    }
}
