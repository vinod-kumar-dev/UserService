using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.ViewModels
{
    public class CreateFoodModels
    {
        public string Name { get; set; } = string.Empty!;
        public string Code { get; set; } = string.Empty!;
        public decimal Price { get; set; }
        public string Path { get; set; } = string.Empty!;
    }
    public class UpdateFoodModels
    {
    }

    public class FoodModels : CreateFoodModels
    {
        public long Id { get; set; }
    }

}
