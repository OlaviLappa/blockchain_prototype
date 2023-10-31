using System.Security.Cryptography;
using System.Text;

namespace blockchain_prototype.Transaction
{
    public class Signature<T> where T : ITransaction
    {
        private T _transactionHelper;

        /// <summary>
        /// Данный класс предназначен для осуществления подписи транзакции
        /// </summary>
        /// <param name="transaction">Принимает объект типа "Transaction", в котором находятся данные проводимой транзакции</param>
        /// <param name="transactionHelper">Принимает объект типа "ITransaction", который содержит вспомогательные методы</param>
        public Signature(T transactionHelper)
        {
            _transactionHelper = transactionHelper;
        }

        public byte[] SignTransaction(blockchain_prototype.Entities.Transaction transaction, string privateKeyHex)
        {
            byte[] privateKeyBytes = _transactionHelper.HexToBytes(privateKeyHex);

            byte[] transactionData = Encoding.UTF8.GetBytes($"{transaction.TransactionId}{transaction.TimeStamp}" +
                $"{transaction.TransactionType}{transaction.Amount}{transaction.Status}{transaction.Sender}{transaction.Recipient}");

            byte[] signature;

            using (ECDsa ecdsa = ECDsa.Create())
            {
                //Import private key
                ECParameters privateKeyParams = new ECParameters
                {
                    D = privateKeyBytes,
                    Curve = ECCurve.NamedCurves.nistP256,
                };

                ecdsa.ImportParameters(privateKeyParams);

                signature = ecdsa.SignData(transactionData, HashAlgorithmName.SHA256);
            }

            return signature;
        }
    }
}