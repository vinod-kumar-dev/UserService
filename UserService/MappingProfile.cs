using AutoMapper;
using UserService.Models;

namespace UserService
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<MoneyTransferCommand, MoneyTransferCommandModel>();
            CreateMap<MoneyTransferCommandModel, MoneyTransferCommand>();
        }
    }
}
