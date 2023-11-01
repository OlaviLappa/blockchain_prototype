using blockchain_prototype;
using blockchain_prototype.Entities;
using blockchain_prototype.Network;
using blockchain_prototype.Transaction;

namespace BlockchainConsoleApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Blockchain blockchain = new Blockchain();
                SystemInitialization systemInitialization = new SystemInitialization();

                 List<string> testSeedPhrase1 = new List<string>()
                {
                    "Генерация тестовых ключей и счёта",
                };

                List<string> testSeedPhrase2 = new List<string>()
                {
                    "опасность_",
                    "помощь_",
                    "холм_",
                    "комфорт",
                    "оружие",
                    "запасной",
                    "жидкость",
                    "идеальный",
                    "отдать честь",
                    "зависеть",
                    "Форум",
                    "имитировать_"
                };
                
                BalanceHandler balanceHandler = new BalanceHandler();

                Wallet senderWallet = 
                    //new Wallet("f2a61caed477d63e8863d730e7ef836cd06a7c524836fdefa77ab323953392c7");
                    new Wallet("1997fd992fedb670ba3db82feb2e72e0043d169dd5f5608a9e5f7ba9c8427a90");

                senderWallet.PrivateKeyHex = "24bfc73dee31400b36b7be5472ce290a4b49f88571f2effc046a8b8960283352";
                senderWallet.PublicKeyHex = "5030937adc53413b7ed0fba9f081fbc9fc25b3b2b39543a87d09472e9f8eefc70a8f303ea283b2b6d0d80ad8e6ca28ef0afabfdf4a6e4efcf2ffa1cda5f5ac82";
                senderWallet.Balance = balanceHandler.GetCurrentBalance(senderWallet);

                Wallet recepientWallet = 
                    //new Wallet("1997fd992fedb670ba3db82feb2e72e0043d169dd5f5608a9e5f7ba9c8427a90");
                    new Wallet("f2a61caed477d63e8863d730e7ef836cd06a7c524836fdefa77ab323953392c7");
                recepientWallet.Balance = balanceHandler.GetCurrentBalance(recepientWallet);

                Transaction transaction = new Transaction(senderWallet, recepientWallet, 100, TransactionType.Transfer);  
                ITransaction additional = new Hex();

                Signature<ITransaction> sign = new Signature<ITransaction>(additional);
                byte[] signature = sign.SignTransaction(transaction, senderWallet.PrivateKeyHex);
                transaction.Signature = signature;

                systemInitialization.AddTransactionToPool(transaction);

                await systemInitialization.Launch();
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.ToString());
            }

            Console.ReadKey();
        }
    }
}