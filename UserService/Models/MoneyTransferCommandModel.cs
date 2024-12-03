using MediatR;

namespace UserService.Models
{
    public class MoneyTransferCommandModel:IRequest
    {
        public int TransferAmount { get; set; }
        public string RecipientName { get; set; }
        public string SenderName { get; set; }
    }
    public class MoneyTransferCommand
    {
        public int TransferAmount { get; set; }
        public string RecipientName { get; set; }
        public string SenderName { get; set; }
        public int TransactionId { get; set; }
    }
}
