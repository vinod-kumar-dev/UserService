using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace UserService.Models
{
    public class UserModel
    {
        public string FirstName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string PhoneNum { get; set; } = string.Empty!;

    }
    public class ViewUserModel : UserModel
    {
        public string Role { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public long Id { get; set; }
    }

    public class UpdateUserModel : UserModel
    {

    }
    public class CreateUserModel : UserModel
    {
        public string Role { get; set; } = string.Empty!;
        public string EmailId { get; set; } = string.Empty!;
        public string PasswordHash { get; set; } = string.Empty!;
    }
    public class FilterRequest<T>
    {
        public int PageNum{ get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; } = string.Empty;
        public string SortType { get; set; } = "desc";
        public T AdditionalData { get; set; }
    }
    public class UserRequest
    {
        public string Role { get; set; } = string.Empty;
    }

}
