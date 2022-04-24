
namespace Domain.Transactions
{
    public interface IFileProcessor
    {
        List<string> ErrorMessages { get; }
        List<Transaction> Transactions { get; }

        void ReadAndValidateFile();
        void StoreTransactions();
    }
}