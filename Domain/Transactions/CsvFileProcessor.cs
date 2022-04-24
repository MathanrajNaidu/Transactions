using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Transactions
{
    public class CsvFileProcessor : IFileProcessor
    {
        private StreamReader _reader;

        public List<string> ErrorMessages { get; private set; }

        public List<Transaction> Transactions { get; private set; }


        private CsvFileProcessor(StreamReader reader)
        {
            _reader = reader;
            ErrorMessages = new List<string>();
            Transactions = new List<Transaction>();
        }

        public static CsvFileProcessor Create(StreamReader reader)
        {

            CsvFileProcessor csvFileProcessor = new(reader);
            csvFileProcessor._reader = reader;
            return csvFileProcessor;
        }

        public void ReadAndValidateFile()
        {
            while (!_reader.EndOfStream)
            {
                var line = _reader.ReadLine();
                var cols = line?.Split(',');
                if (cols?.Length >= 0)
                {
                    Transaction transaction = new Transaction();
                    if (AddErrorMessageIfNull(cols[0]))
                    {
                        transaction.Id = cols[0].ToString();
                    }
                    if (AddErrorMessageIfNull(cols[1]))
                    {
                        if (Decimal.TryParse(cols[1].ToString(), out decimal amount))
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
                        if (DateTime.TryParseExact(cols[3].ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            transaction.TransactionDate = parsedDate;
                        }
                        else
                        {
                            ErrorMessages.Add($"Transaction Date not able to parse {cols[3]}");
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
                }
            }
        }

        private readonly List<string> _status = new()
        {
            "Approved",
            "Failed",
            "Finished"
        };

        public void StoreTransactions()
        {
            if (ErrorMessages.Count == 0)
            {

            }
            else
            {

            }
        }

        private bool AddErrorMessageIfNull(string col)
        {
            if (col == null)
            {
                ErrorMessages.Add($"{col} is null");
                return false;
            }

            return true;
        }

        private static bool IsValidCurrencyCode(string currencyCode)
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                          .Select(x => new RegionInfo(x.LCID));
            return regions.Any(x => x.ISOCurrencySymbol == currencyCode);
        }
    }
}
