using AutoMapper;
using Domain.Interfaces;
using Domain.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionRepository _transactionRepository;
        private IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        [Route("GetAllByCurrency/{currencyCode}")]
        [HttpGet]
        public IActionResult GetAllByCurrency (string currencyCode)
        {
            var transactions = _transactionRepository
                .GetTransactions()
                .Where(x => x.CurrencyCode.ToLower() == currencyCode)
                .ToList();

            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

            return new OkObjectResult(result);
        }

        [Route("GetAllByStatus/{status}")]
        [HttpGet]
        public IActionResult GetAllByStatus(string status)
        {
            var transactions = _transactionRepository
                .GetTransactions()
                .Where(x => x.Status.ToLower() == status)
                .ToList();

            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

            return new OkObjectResult(result);
        }


        [Route("GetAllByUnifiedStatus/{unifiedStatus}")]
        [HttpGet]
        public IActionResult GetAllByUnifiedStatus(string unifiedStatus)
        {
            var statuses = GetStatusesByUnifiedStatus(unifiedStatus);
            var transactions = _transactionRepository
                .GetTransactions()
                .Where(x => statuses.Contains(x.Status.ToLower()))
                .ToList();

            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

            return new OkObjectResult(result);
        }

        [Route("GetAllByDateRange")]
        [HttpGet]
        public IActionResult GetAllByDateRange(DateTime startDate, DateTime? endDate)
        {
            if(endDate == null) endDate = DateTime.MaxValue;
            var transactions = _transactionRepository
                .GetTransactions()
                .Where(x => x.TransactionDate >= startDate && x.TransactionDate <=endDate)
                .ToList();

            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

            return new OkObjectResult(result);
        }

        private List<string> GetStatusesByUnifiedStatus(string unifiedStatus)
        {
            List<string> statuses = new();
            switch (unifiedStatus.ToLower())
            {
                case "a":
                    {
                        statuses.Add("approved");
                        break;
                    }
                case "r":
                    {
                        statuses.Add("failed"); 
                        statuses.Add("rejected");
                        break;
                    }
                case "d":
                    {
                        statuses.Add("finished");
                        statuses.Add("done");
                        break;
                    }
            };
            return statuses;
        }
    }
}
