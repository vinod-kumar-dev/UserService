using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Helper;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingController : ControllerBase
    {
        private readonly ILogger<BankingController> _logger;
       // private readonly IMediator _mediator;
        private readonly RabbitMQHelper _rabbitMQHelper;

        public BankingController(ILogger<BankingController> logger, RabbitMQHelper rabbitMQHelper)
        {
            _logger = logger;
           // _mediator = mediator;
            _rabbitMQHelper = rabbitMQHelper;
        }
        //[HttpPost(Name = "TransferMoney")]
        //public async Task Post([FromBody] MoneyTransferCommandDto moneyTransfer)
        //{
        //    _logger.LogInformation($"Request received from {moneyTransfer.SenderName}");

        //    await _mediator.Send(moneyTransfer);
        //}
        [HttpPost(Name = "produce/message")]
        public async Task<IActionResult> ProduceMessage(string message)
        {
            await _rabbitMQHelper.PublishMessage("queue1", "Firsmessage posted");
            return Ok("");
        }
    }
}
