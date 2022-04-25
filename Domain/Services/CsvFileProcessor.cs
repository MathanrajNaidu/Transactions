using System.Globalization;
using Domain.Interfaces;
using Domain.Transactions;

namespace Domain.Processors
{
    public class CsvFileProcessor : FileProcessorBase, IFileProcessor
    {
        public CsvFileProcessor(StreamReader reader, ITransactionRepository transactionRepository)
        {
            _reader = reader;
            _transactionRepository = transactionRepository;
            ErrorMessages = new List<string>();
            Transactions = new List<Transaction>();
        }

        public void ReadAndValidateFile()
        {
            while (!_reader.EndOfStream)
            {
                var line = _reader.ReadLine();
                var cols = line?.Split(',');
                if (cols?.Length >= 0)
                {
                    Transaction transaction = new();
                    if (AddErrorMessageIfNull(cols[0]))
                    {
                        transaction.Id = cols[0].ToString();
                        if(transaction.Id.Length > 50)
                        {
                            ErrorMessages.Add($"Transaction id more than max length {cols[1]}");
                        }
                    }
                    if (AddErrorMessageIfNull(cols[1]))
                    {
                        if (decimal.TryParse(cols[1].ToString(), out decimal amount))
                        {
                            transaction.Amount = amount;
                        }
                        else
                        {
                            ErrorMessages.Add($"Amount not decimal {cols[1]}");
                        }
                    }
                    if (AddErrorMessageIfNull(cols[2]))
                    {
                        transaction.CurrencyCode = cols[2].ToString();
                        if (!IsValidCurrencyCode(transaction.CurrencyCode))
                        {
                            ErrorMessages.Add($"Is not valid Currency Symbol {transaction.CurrencyCode}");
                        };
                    }
                    if (AddErrorMessageIfNull(cols[3]))
                    {
                        if (DateTime.TryParseExact(cols[3].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            transaction.TransactionDate = parsedDate;
                        }
                        else
                        {
                            ErrorMessages.Add($"Transaction Date is not in dd/MM/yyyy hh:mm:ss format {cols[3]}");
                        };
                    }
                    if (AddErrorMessageIfNull(cols[4]))
                    {
                        transaction.Status = cols[4].ToString();
                        if (!_status.Any(x => x == transaction.Status))
                        {
                            ErrorMessages.Add($"Status is not available {transaction.Status}");
                        }
                    }

                    Transactions.Add(transaction);
                }
            }
        }

        private readonly List<string> _status = new()
        {
            "Approved",
            "Failed",
            "Finished"
        };

       
    }
}
