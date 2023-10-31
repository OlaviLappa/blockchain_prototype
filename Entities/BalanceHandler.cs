namespace blockchain_prototype.Entities
{
    public class BalanceHandler
    {
        public decimal GetCurrentBalance(Wallet wallet)
        {
            DataFile dataFile = new DataFile();
            List<Block> blocks = dataFile.GetChain();

            decimal balance = wallet.Balance;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].Data == "Genesis Block")
                {
                    continue;
                }

                if (blocks[i].Transaction.Sender.Wallet.Address == wallet.Address)
                {
                    balance -= blocks[i].Transaction.Amount;
                }

                else if (blocks[i].Transaction.Recipient.Wallet.Address == wallet.Address)
                {
                    balance += blocks[i].Transaction.Amount;
                }
            }

            return balance;
        }

        /// <summary>
        /// Данный метод предназначен для проверки состояния токена в данный момент времени. Например 
        /// был создан смарт-контракт на 100000 токенов, метод проверяет все блоки цепочки и сверяет суммы
        /// тех или иных транзакций, чтобы удостовериться, не было ли выхода за пределы сгенерированной суммы токенов
        /// </summary>
        /// <param name="contractAddress"></param>
        /// <param name="tokensAmount"></param>
        /// <returns>Сумма токенов контракта (decimal)</returns>
        public decimal CheckTokenState(string contractAddress, decimal tokensAmount)
        {
            DataFile dataFile = new DataFile();
            List<Block> blocks = dataFile.GetChain();

            decimal balanceForCheck = 0;
            decimal lastStateOfBlc = 0;
            decimal temp;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].Data == "Genesis Block" || i == 1)
                {
                    continue;
                }

                if (blocks[i].Transaction.Sender.Wallet.Address == contractAddress)
                {
                    balanceForCheck += blocks[i].Transaction.Amount;
                    lastStateOfBlc = blocks[i].Transaction.Sender.Wallet.Balance + balanceForCheck;
                }
            }

            temp = lastStateOfBlc - tokensAmount;

            return lastStateOfBlc - temp;
        }
    }
}