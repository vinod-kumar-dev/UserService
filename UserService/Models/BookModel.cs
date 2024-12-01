using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace UserService.Models
{
    public class BookModel
    {
        public string Name { get; set; } = string.Empty!;
        public string Author { get; set; } = string.Empty!;
        public string Unit { get; set; } = string.Empty!;
        public double Price { get; set; }

    }
    public class ViewBookModel : BookModel
    {
        public long Id { get; set; }
    }
}
