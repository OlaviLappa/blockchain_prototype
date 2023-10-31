using System.Security.Cryptography;
using System.Text;


namespace blockchain_prototype.Transaction
{
    public class Verification<T> where T : ITransaction
    {
        private T _transactionHelper;

        /// <summary>
        /// Данный класс предназначен для проверки подписи транзакции
        /// </summary>
        public Verification(T transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        public bool VerifyTransaction(blockchain_prototype.Entities.Transaction transaction, string publicKeyHex)
        {
            byte[] publicKeyBytes = _transactionHelper.HexToBytes(publicKeyHex);

            byte[] transactionData = Encoding.UTF8.GetBytes($"{transaction.TransactionId}{transaction.TimeStamp}" +
                $"{transaction.TransactionType}{transaction.Amount}{transaction.Status}{transaction.Sender}{transaction.Recipient}");

            using (ECDsa ecdsa = ECDsa.Create())
            {
                ECParameters publicKeyParams = new ECParameters
                {
                    Q = new ECPoint
                    {
                        X = publicKeyBytes.AsSpan(0, 32).ToArray(),
                        Y = publicKeyBytes.AsSpan(32, 32).ToArray()
                    },

                    Curve = ECCurve.NamedCurves.nistP256,
                };

                ecdsa.ImportParameters(publicKeyParams);

                bool isValid = ecdsa.VerifyData(transactionData, transaction.Signature, HashAlgorithmName.SHA256);

                return isValid;
            }
        }
    }
}
