using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace blockchain_prototype.Entities
{
    public class Wallet
    {
        public string Address { get; set; }
        public string PrivateKeyHex { get; set; }
        public string PublicKeyHex { get; set; }
        public List<string> SeedPhrase { get; set; }

        public decimal Balance
        {
            get
            {
                return _balance;
            }

            set
            {
                _balance = value;
            }
        }

        private decimal _balance;

        public Wallet() { }

        public Wallet(string address)
        {
            Address = address;
            _balance = 0;
        }

        public Wallet(List<string> seedPhrase)
        {
            (byte[] privateKey, byte[] publicKey) = KeysGeneration(seedPhrase);

            PrivateKeyHex = BitConverter.ToString(privateKey).Replace("-", "").ToLower();
            PublicKeyHex = BitConverter.ToString(publicKey).Replace("-", "").ToLower();
            SeedPhrase = seedPhrase;

            Address = GenerateAddress(publicKey);
            _balance = 0;

            BalanceHandler balanceHandler = new BalanceHandler();
            decimal value = balanceHandler.GetCurrentBalance(this);

            if (value > 0)
            {
                _balance = value;
            }

            else
            {
                _balance = 0;
            }
        }

        public string GenerateAddress(byte[] publicKey)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(publicKey);
                string address = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return address;
            }
        }

        private (byte[] privateKey, byte[] publicKey) KeysGeneration(List<string> seedPhrase)
        {
            byte[] seedBytes = Encoding.UTF8.GetBytes(string.Join(" ", seedPhrase));

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] masterKey = sha256.ComputeHash(seedBytes);

                using (ECDsa ecdsa = ECDsa.Create())
                {
                    ECParameters privateKeyParams = new ECParameters
                    {
                        D = masterKey,
                        Curve = ECCurve.NamedCurves.nistP256,
                    };

                    ecdsa.ImportParameters(privateKeyParams);

                    ECParameters publicKeyParams = ecdsa.ExportParameters(false);

                    byte[] publicKeyBytes = new byte[64];

                    Array.Copy(publicKeyParams.Q.X, publicKeyBytes, 32);
                    Array.Copy(publicKeyParams.Q.Y, 0, publicKeyBytes, 32, 32);

                    return (masterKey, publicKeyBytes);
                }
            }
        }

        public bool CheckBalance(Transaction transaction)
        {
            bool isChanged;

            if(transaction.Sender.Wallet.Balance != 0 && transaction.Sender.Wallet.Balance >= transaction.Amount)
            {
                if(transaction.Amount <= 0)
                {
                    isChanged = false;
                }

                else
                {
                    transaction.Sender.Wallet.Balance -= transaction.Amount;
                    transaction.Recipient.Wallet.Balance += transaction.Amount;

                    isChanged = true;
                }
            }

            else
            {
                isChanged = false;
            }

            return isChanged;
        }
    }
}