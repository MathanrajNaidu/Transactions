
namespace Domain.Interfaces
{
    public interface IFileProcessor
    {
        List<string> ErrorMessages { get; }
        void ReadAndValidateFile();
        void StoreTransactions();
    }
}