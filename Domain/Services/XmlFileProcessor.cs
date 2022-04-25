using Domain.Interfaces;
using Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Domain.Processors
{
    public class XmlFileProcessor : FileProcessorBase, IFileProcessor
    {
        public XmlFileProcessor(StreamReader reader, ITransactionRepository transactionRepository)
        {
            _reader = reader;
            _transactionRepository = transactionRepository;
            ErrorMessages = new List<string>();
            Transactions = new List<Transaction>();
            InvalidTransactions = new List<Transaction>();
        }

        public void ReadAndValidateFile()
        {
            using var reader = XmlReader.Create(_reader);
            Transaction transaction = new();
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {

                    switch (reader.Name.ToString())
                    {
                        case "Transaction":
                            {
                                var transactionId = reader.GetAttribute("id")?.ToString();
                                if (AddErrorMessageIfNull(transactionId))
                                {
                                    transaction = new();
                                    InvalidRecord = false;
                                    transaction.Id = transactionId;
                                    if (transaction.Id.Length > 50)
                                    {
                                        AddErrorMessage($"Transaction id more than max length {transactionId}");
                                    }
                                }
                                break;
                            }
                        case "TransactionDate":
                            {
                                var transactionDate = reader.ReadElementContentAsString();
                                if (AddErrorMessageIfNull(transactionDate))
                                {
                                    if (DateTime.TryParseExact(transactionDate, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        transaction.TransactionDate = parsedDate;
                                    }
                                    else
                                    {
                                        AddErrorMessage($"Transaction Date is not in yyyy-MM-ddThh:mm:ss format {transactionDate}");
                                    };
                                }
                                break;
                            }
                        case "PaymentDetails":
                            {
                                using var subtreeReader = reader.ReadSubtree();
                                while (subtreeReader.Read())
                                {
                                    if (subtreeReader.IsStartElement())
                                    {
                                        switch (subtreeReader.Name.ToString())
                                        {
                                            case "Amount":
                                                {
                                                    var amount = subtreeReader.ReadElementContentAsString();
                                                    if (AddErrorMessageIfNull(amount))
                                                    {
                                                        if (decimal.TryParse(amount, out decimal amountD))
                                                        {
                                                            transaction.Amount = amountD;
                                                        }
                                                        else
                                                        {
                                                            AddErrorMessage($"Amount not decimal {amount}");
                                                        }
                                                    }
                                                    break;
                                                }
                                            case "CurrencyCode":
                                                {
                                                    var currenyCode = reader.ReadElementContentAsString();
                                                    if (AddErrorMessageIfNull(currenyCode))
                                                    {
                                                        transaction.CurrencyCode = currenyCode;
                                                        if (!IsValidCurrencyCode(transaction.CurrencyCode))
                                                        {
                                                            AddErrorMessage($"Is not valid Currency Symbol {transaction.CurrencyCode}");
                                                        };
                                                    }
                                                    break;
                                                }
                                        }
                                    }
                                }
                                  
                                break;
                            }
                        case "Status":
                            {
                                var status = reader.ReadElementContentAsString();
                                if (AddErrorMessageIfNull(status))
                                {
                                    transaction.Status = status;
                                    if (!_status.Any(x => x == transaction.Status))
                                    {
                                        AddErrorMessage($"Status is not available {transaction.Status}");
                                    }
                                    Transactions.Add(transaction);
                                    if (InvalidRecord) InvalidTransactions.Add(transaction);
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

    }
}
