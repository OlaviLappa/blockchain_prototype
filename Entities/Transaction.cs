namespace blockchain_prototype.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public Sender? Sender { get; set; } 
        public Recipient? Recipient { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        public Status Status { get; set; }
        public TransactionType TransactionType { get; set; }
        public byte[] Signature { get; set; }
        
        public Transaction(Wallet senderAddress, Wallet recepientAddress, decimal amount, TransactionType transactionType)
        {
            Sender = new Sender()
            {
                Wallet = senderAddress
            };

            Recipient = new Recipient()
            {
                Wallet = recepientAddress
            };

            Amount = amount;
            TimeStamp = DateTime.UtcNow;
            TransactionType = transactionType;
        }

        public Transaction() { }
    }
}