using System.Security.Cryptography;
using System.Text;

namespace blockchain_prototype
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public blockchain_prototype.Entities.Transaction Transaction { get; set; }

        public Block(DateTime timestamp, string data, string previousHash, blockchain_prototype.Entities.Transaction transaction)
        {
            Index = 0;
            Timestamp = timestamp;
            Data = data;
            PreviousHash = previousHash;
            Hash = CalculateHash();
            Transaction = transaction;
        }

        public string CalculateHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes($"{Timestamp}-{Data}-{PreviousHash}");
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}