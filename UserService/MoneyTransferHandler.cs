using AutoMapper;
using MediatR;
using UserService.Helper;
using UserService.Models;

namespace UserService
{
    public class MoneyTransferHandler:IRequestHandler<MoneyTransferCommandModel>
    {
        private readonly IMapper _mapper;
        private readonly IRabbitMqBusHelper _rabbitMqBus;

        public MoneyTransferHandler(IMapper mapper, IRabbitMqBusHelper rabbitMqBus)
        {
            _mapper = mapper;
            _rabbitMqBus = rabbitMqBus;
        }

        public async Task Handle(MoneyTransferCommandModel request, CancellationToken cancellationToken)
        {
            var moneyTransferCommand = _mapper.Map<MoneyTransferCommand>(request);

            //await _rabbitMqBus.Publish(moneyTransferCommand);
        }
    }
}
