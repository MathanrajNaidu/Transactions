
using Domain.Transactions;

namespace Domain.Interfaces
{
    public interface IFileProcessor
    {
        List<string> ErrorMessages { get; }
        List<Transaction> InvalidTransactions { get; }
        void ReadAndValidateFile();
        void StoreTransactions();
    }
}