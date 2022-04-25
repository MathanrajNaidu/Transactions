using AutoMapper;
using Domain.Transactions;
using WebApplication2.Models;

namespace WebApplication2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionResponseDto>();
        }
    }
}
