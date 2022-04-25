using Domain.Transactions;

namespace Domain.Interfaces
{ 
    public interface ITransactionRepository :IDisposable
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionByID(int transactionId);
        void InsertTransaction(Transaction transaction);
        void DeleteTransaction(int transactionId);
        void UpdateTransaction(Transaction transaction);
        void Save();
    }
}
