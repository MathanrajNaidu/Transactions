﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Transactions
{
    public class Transaction
    {

        public string Id { get; set; }

        public decimal Amount { get; set; }
        
        public string CurrencyCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Status { get; set; }


    }
}
