using UserService.Models;
using UserService.ViewModels;

namespace UserService.Interfaces
{
    public interface IBook
    {
        Task<long> Add(BookModel rquest);
        Task<long> Update(long id, BookModel rquest);
        Task<ViewBookModel> GetById(long id);
    }
}
