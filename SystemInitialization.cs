using blockchain_prototype.Entities;
using blockchain_prototype.Exceptions;
using blockchain_prototype.Transaction;

namespace blockchain_prototype
{
    public class SystemInitialization
    {
        private object waiter = new object();
        private Blockchain blockchain;
        private TransactionPool pool;
        private List<blockchain_prototype.Entities.Transaction> transactions;

        private int _threadAmount = 10;
        private int _maxTransactionsAmount = 30;

        private int _awaitingHandleTransactionsTime = 5000;
        private int _addTransactionsIntoChainTime = 12000;

        public SystemInitialization()
        {
            blockchain = new Blockchain();
            pool = new TransactionPool();
            transactions = new List<blockchain_prototype.Entities.Transaction>();
        }

        public async Task Launch()
        {
            while (true)
            {
                HandleTransaction();

                await Task.Delay(_awaitingHandleTransactionsTime);

                if(pool.GetTransactionsCount() > _maxTransactionsAmount)
                {
                    for (int i = 0; i < _threadAmount; i++)
                    {
                        new Thread(() => HandleTransaction()).Start();
                    }
                }
            }
        }

        public void AddTransactionToPool(blockchain_prototype.Entities.Transaction transaction)
        {
            pool.AddTransaction(transaction);
        }

        public string ShowTestResult(blockchain_prototype.Entities.Transaction nextTransaction)
        {
            BalanceHandler balanceHandler = new BalanceHandler();

            decimal t = balanceHandler.CheckTokenState("1997fd992fedb670ba3db82feb2e72e0043d169dd5f5608a9e5f7ba9c8427a90", 10000000000);
            Console.WriteLine(t);

            foreach (Block item in blockchain.Chain)
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("_______________________________________________________________________________");
                Console.WriteLine("..................................................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine($"Transaction ID: {item.Index}");
                Console.WriteLine($"Data: {item.Data}");
                Console.WriteLine($"Timestamp: {item.Timestamp}");
                Console.WriteLine($"Hash: {item.Hash}");
                Console.WriteLine($"Previous Hash: {item.PreviousHash}");
                Console.WriteLine("..................................................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine("......................Additional data:........................");
                Console.WriteLine("..................................................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine($"From: {nextTransaction.Sender.Wallet.Address}");
                Console.WriteLine($"To: {nextTransaction.Recipient.Wallet.Address}");
                Console.WriteLine($"Amount: {nextTransaction.Amount}");
                Console.WriteLine($"Transaction signature: {nextTransaction.Signature}");
                Console.WriteLine("..................................................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine($"Sender balance: {nextTransaction.Sender.Wallet.Balance}");
                Console.WriteLine($"Recepient balance: {nextTransaction.Recipient.Wallet.Balance}");
                Console.WriteLine("..................................................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine($"_UbToken amount: {t}.............................................");
                Console.WriteLine("..................................................................");
                Console.WriteLine("__________________________________________________________________________________");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }

            Result result = new Result(blockchain);
            Block block = result.GetLastBlock();

            string json = blockchain.dataFile.SerializeToJsonObject(block);

            return json;
        }

        private async void HandleTransaction()
        {
            blockchain_prototype.Entities.Transaction nextTransaction = await pool.GetNextTransactionAsync();

            lock (waiter)
            {
                if (nextTransaction != null)
                {
                    string msg;
                    string message1 = "Подпись транзакции недействительна!";
                    string message2 = "Некорретные данные при вводе, либо недостаточно средств. Транзакция отклонена.";

                    ITransaction additional = new Hex();
                    Verification<ITransaction> verify = new Verification<ITransaction>(additional);

                    bool status = verify.VerifyTransaction(nextTransaction, nextTransaction.Sender.Wallet.PublicKeyHex);

                    if (status)
                    {
                        Wallet change = new Wallet();

                        if(change.CheckBalance(nextTransaction))
                        {
                            DestroyPrivateData(nextTransaction);

                            nextTransaction.Status = Status.Success;

                            blockchain.AddBlock(new Block(DateTime.Now, $"{nextTransaction.TransactionId}{nextTransaction.TimeStamp}" +
                                $"{nextTransaction.TransactionType}{nextTransaction.Amount}{nextTransaction.Status}{nextTransaction.Sender.Wallet}" +
                                $"{nextTransaction.Recipient.Wallet}", "", nextTransaction), out msg);
                        }

                        else
                        {
                            nextTransaction.Status = Status.Failed;
                            throw new InsufficientFundsException(message2);
                        }
                    }

                    else
                    {
                        nextTransaction.Status = Status.Failed;
                        throw new InvalidTransactionException(message1);
                    }

                    Console.WriteLine(msg);

                    ShowTestResult(nextTransaction);
                }

                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("Blockchain node 0.0.1: Awaiting incoming transactions");
                    Console.WriteLine($"Current date: {DateTime.Now}");
                    Console.WriteLine($"Current blocks height: {blockchain.Chain.Count}");
                    Console.WriteLine("");
                }

                Console.WriteLine($"Transactions in pool: {pool.GetTransactionsCount()}");
            }
        }

        private void DestroyPrivateData(blockchain_prototype.Entities.Transaction nextTransaction)
        {
            nextTransaction.Sender.Wallet.PublicKeyHex = null;
            nextTransaction.Sender.Wallet.PrivateKeyHex = null;
            nextTransaction.Sender.Wallet.SeedPhrase = null;

            nextTransaction.Recipient.Wallet.PublicKeyHex = null;
            nextTransaction.Recipient.Wallet.PrivateKeyHex = null;
            nextTransaction.Recipient.Wallet.SeedPhrase = null;
        }

        private void AddTransactionsIntoBlock(List<blockchain_prototype.Entities.Transaction> transactions)
        {

        }
    }
}