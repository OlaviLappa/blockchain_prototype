namespace blockchain_prototype.Entities
{
    public class Recipient : IRecipient
    {
        public Wallet Wallet { get; set; }
        public string Address { get; set; }
        public string? Name { get; set; }
        public DateTime? DateAccountCreating { get; set; }
    }
}