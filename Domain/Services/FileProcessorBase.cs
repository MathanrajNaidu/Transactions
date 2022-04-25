using Domain.Interfaces;
using Domain.Transactions;
using System.Globalization;

namespace Domain.Processors
{
    public class FileProcessorBase
    {
        protected StreamReader _reader;

        protected ITransactionRepository _transactionRepository;

        public List<string> ErrorMessages { get; protected set; }

        protected List<Transaction> Transactions { get; set; }

        protected static bool IsValidCurrencyCode(string currencyCode)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                          .Select(x => new RegionInfo(x.LCID));
            return regions.Any(x => x.ISOCurrencySymbol == currencyCode);
        }

        protected bool AddErrorMessageIfNull(string col)
        {
            if (col == null)
            {
                ErrorMessages.Add($"{col} is null");
                return false;
            }

            return true;
        }

        public void StoreTransactions()
        {
            foreach (Transaction transaction in Transactions)
            {
                _transactionRepository.InsertTransaction(transaction);
            }
            try
            {
                _transactionRepository.Save();
            } 
            catch (Exception ex)
            {
                ErrorMessages.Add(ex.InnerException.Message);
            }
        }
    }
}