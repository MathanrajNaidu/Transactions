namespace WebApplication2.Models
{
    public class TransactionResponseDto
    {
        private decimal amount;
        private string currencyCode;
        private string status;
        public string Id { get; set; }
        public string Payment
        {
            get
            {
                return $"{amount} {currencyCode}";
            }

            private set { }
        }
        public string Status { 
            get
            {
                string outputStatus = string.Empty;
                switch (status)
                {
                    case "Approved":
                        {
                            outputStatus = "A";
                            break;
                        }
                    case "Failed":
                    case "Rejected":
                        {
                            outputStatus = "R";
                            break;
                        }
                    case "Finished":
                    case "Done":
                        {
                            outputStatus = "D";
                            break;
                        }
                };
                return outputStatus;
            }
            set
            {
                status = value;
            }
        }
        public decimal Amount
        {
            set
            {
                amount = value;
            }
        }

        public string CurrencyCode
        {
            set
            {
                currencyCode = value;
            }
        }
    }   
}
