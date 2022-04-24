using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Domain.Transactions
{
    public class XmlFileProcessor : IFileProcessor
    {
        private StreamReader _reader;

        public List<string> ErrorMessages { get; private set; }

        public List<Transaction> Transactions { get; private set; }


        private XmlFileProcessor(StreamReader reader)
        {
            _reader = reader;
            ErrorMessages = new List<string>();
            Transactions = new List<Transaction>();
        }

        public static XmlFileProcessor Create(StreamReader reader)
        {

            XmlFileProcessor xmlFileProcessor = new(reader);
            xmlFileProcessor._reader = reader;
            return xmlFileProcessor;
        }

        public void ReadAndValidateFile()
        {
            using var reader = XmlReader.Create(_reader);
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    Transaction transaction = new Transaction();
                    transaction.Id = reader.GetAttribute("Transaction Id")?.ToString();
                    switch (reader.Name.ToString())
                    {
                        case "TransactionDate":
                            {
                                if (AddErrorMessageIfNull(reader.ReadContentAsString()))
                                {
                                    if (DateTime.TryParseExact(reader.ReadContentAsString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        transaction.TransactionDate = parsedDate;
                                    }
                                    else
                                    {
                                        ErrorMessages.Add($"Transaction Date not able to parse {reader.ReadContentAsString()}");
                                    };
                                }
                                break;
                            }
                        case "PaymentDetails":
                            {
                                var subTree = reader.ReadSubtree();
                                while (subTree.Read())
                                {
                                    switch (subTree.Name.ToString())
                                    {
                                        case "Amount":
                                            {
                                                if (AddErrorMessageIfNull(subTree.ReadContentAsString()))
                                                {
                                                    if (Decimal.TryParse(subTree.ReadContentAsString(), out decimal amount))
                                                    {
                                                        transaction.Amount = amount;
                                                    }
                                                    else
                                                    {
                                                        ErrorMessages.Add($"Amount not decimal {subTree.ReadContentAsString()}");
                                                    }
                                                }
                                                break;
                                            }
                                        case "PaymentDetails":
                                            {
                                                transaction.CurrencyCode = subTree.ReadContentAsString();
                                                if (!IsValidCurrencyCode(transaction.CurrencyCode))
                                                {
                                                    ErrorMessages.Add($"Is not valid Currency Symbol {transaction.CurrencyCode}");
                                                };
                                                break;
                                            }
                                    }
                                }
                                break;
                            }
                        case "Status":
                            {
                                if (AddErrorMessageIfNull(reader.ReadContentAsString()))
                                {
                                    transaction.Status = reader.ReadContentAsString();
                                    if (!_status.Any(x => x == transaction.Status))
                                    {
                                        ErrorMessages.Add($"Status is not available {transaction.Status}");
                                    }
                                }
                                break;
                            }
                    }
                }
            }
        }

        private readonly List<string> _status = new()
        {
            "Approved",
            "Rejected",
            "Done"
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
