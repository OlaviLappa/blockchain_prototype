namespace blockchain_prototype.SmartContracts
{
    public class Token
    {
        public int ContractId { get; set; }
        public string ContractAddress { get; set; }
        public decimal TokenAmount { get; set; }
        public string TokenName { get; set; }
        public string TokenShortName { get; set; }
    }
}