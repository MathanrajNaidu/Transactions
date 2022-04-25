using Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class Logger
    {
        private static Logger _instance;
        private static bool inUse = false;
        private Logger()
        {

        }

        public static Logger Instance {
            get 
            {
                if (_instance == null)
                {
                    return _instance = new Logger();
                } 
                return _instance;
            } 
        }
        public void LogInvalidRecords(List<Transaction> transactions, List<string> errorMessages)
        {
            while (inUse)
            {

            }
            inUse = true;
            using var file = File.Open("InvalidRecords.csv", FileMode.Append, FileAccess.Write);
            using StreamWriter sw = new StreamWriter(file);
            foreach (Transaction transaction in transactions)
            {
                sw.WriteLine($"{transaction.Id},{transaction.Amount},{transaction.CurrencyCode},{transaction.TransactionDate},{transaction.Status}");
            }
            foreach (string errorMessage in errorMessages)
            {
                sw.WriteLine(errorMessage);
            }
            inUse = false;
        }
    }
}
