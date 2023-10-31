using System.Collections.Concurrent;

public class TransactionPool
{
    private ConcurrentQueue<blockchain_prototype.Entities.Transaction> transactions = new ConcurrentQueue<blockchain_prototype.Entities.Transaction>();
    private SemaphoreSlim semaphore = new SemaphoreSlim(1);

    public void AddTransaction(blockchain_prototype.Entities.Transaction transaction)
    {
        semaphore.Wait();

        try
        {
            transactions.Enqueue(transaction);
        }

        finally
        {
            semaphore.Release();
        }
    }

    public async Task<blockchain_prototype.Entities.Transaction> GetNextTransactionAsync()
    {
        await semaphore.WaitAsync();

        try
        {
            if (transactions.TryDequeue(out var nextTransaction))
            {
                return nextTransaction;
            }

            else
            {
                return null;
            }
        }

        finally
        {
            semaphore.Release();
        }
    }

    public int GetTransactionsCount()
    {
        return transactions.Count;
    }
}